using System.Xml;
using Oligopoly.Squares;

namespace Oligopoly.Writers;

public class HtmlFragmentWriter : Writer, IDisposable
{
    private const string TableRowElement = "tr";
    private const string TableRowDataElement = "td";

    private static XmlWriterSettings? s_settings;

    private readonly XmlWriter _writer;

    private int _byteIndex;
    private bool _disposedValue;

    public HtmlFragmentWriter(TextWriter writer, bool leaveOpen = false)
    {
        if (s_settings is null)
        {
            s_settings = new XmlWriterSettings()
            {
                CloseOutput = !leaveOpen,
                Indent = true,
                OmitXmlDeclaration = true
            };
        }

        _writer = XmlWriter.Create(writer, s_settings);

        _writer.WriteStartElement("table");
        _writer.WriteStartElement("thead");
        _writer.WriteStartElement(TableRowElement);
        WriteTableRowHeader("Byte");
        WriteTableRowHeader("Type");
        WriteTableRowHeader("Value");
        WriteTableRowHeader("Description");
        _writer.WriteFullEndElement();
        _writer.WriteFullEndElement();
        _writer.WriteStartElement("tbody");
    }

    /// <inheritdoc/>
    public override void Write(int value)
    {
        NextByte();
        WriteTableRowData("Integer", rowSpan: 4);
        WriteTableRowData();
        WriteTableRowData();
        _writer.WriteEndElement();
        NextByte();
        _writer.WriteEndElement();
        NextByte();
        _writer.WriteEndElement();
        NextByte();
        _writer.WriteEndElement();
    }

    /// <inheritdoc/>
    public override void Write(SquareType value)
    {
        SquareType[] items = Enum.GetValues<SquareType>();

        NextByte(items.Length);
        WriteTableRowData("Square Type", items.Length);

        for (int i = 0; i < items.Length; i++)
        {
            SquareType item = items[i];

            WriteTableRowDataCode((int)item);
            WriteTableRowData(item.ToString());
            _writer.WriteEndElement();

            if (i < items.Length - 1)
            {
                _writer.WriteStartElement(TableRowElement);
            }
        }
    }

    /// <inheritdoc/>
    public override void Write(string value)
    {
        NextObject();
        WriteTableRowData("String");
        WriteTableRowData();
        WriteTableRowData();
        _writer.WriteEndElement();
    }

    /// <inheritdoc/>
    public override void Write(IWritable value)
    {
        if (_byteIndex is 0)
        {
            WriteVersion();
        }
        else
        {
            NextObject(value.GetType());
            WriteTableRowData();
            WriteTableRowData();
            _writer.WriteEndElement();
        }

        base.Write(value);
    }

    /// <inheritdoc/>
    public override void Write(IReadOnlyCollection<IWritable> value)
    {
        NextByte();
        WriteTableRowData("Integer", rowSpan: 4);
        WriteTableRowData();
        WriteTableRowData("Specifies the number of records in the succeeding collection.", rowSpan: 4);
        _writer.WriteEndElement();
        NextByte();
        _writer.WriteEndElement();
        NextByte();
        _writer.WriteEndElement();
        NextByte();
        _writer.WriteEndElement();
        NextObject(value.GetType());
        WriteTableRowData();
        WriteTableRowData("A collection of data models repeated as many times as specified by the preceding integer value.");
        _writer.WriteEndElement();
    }

    /// <inheritdoc/>
    public override void WriteVersion()
    {
        const string Type = "Byte";

        NextByte();
        WriteTableRowData(Type);
        WriteTableRowDataCode(FormatByte);
        WriteTableRowData("Specifies the format byte.");
        _writer.WriteEndElement();
        NextByte();
        WriteTableRowData(Type);
        WriteTableRowDataCode(VersionByte);
        WriteTableRowData("Specifies the version byte.");
        _writer.WriteEndElement();
    }

    private void NextByte(int rowSpan = 1)
    {
        _writer.WriteStartElement(TableRowElement);
        WriteTableRowData(XmlConvert.ToString(_byteIndex), rowSpan);

        _byteIndex++;
    }

    private void NextObject()
    {
        _writer.WriteStartElement(TableRowElement);
        WriteTableRowData("\u22ee");
    }

    private void NextObject(Type type)
    {
        NextObject();
        _writer.WriteStartElement(TableRowDataElement);
        _writer.WriteStartElement("a");
        _writer.WriteAttributeString("href", '#' + type.Name.TrimEnd('[', ']'));
        _writer.WriteString(type.Name);
        _writer.WriteEndElement();
        _writer.WriteEndElement();
    }

    private void WriteTableRowDataCode(int value)
    {
        _writer.WriteStartElement(TableRowDataElement);
        _writer.WriteElementString("code", XmlConvert.ToString(value));
        _writer.WriteEndElement();
    }

    private void WriteTableRowData(string? value = null, int rowSpan = 1, int columnSpan = 1)
    {
        _writer.WriteStartElement(TableRowDataElement);

        if (rowSpan is not 1)
        {
            _writer.WriteAttributeString("rowspan", XmlConvert.ToString(rowSpan));
        }

        if (columnSpan is not 1)
        {
            _writer.WriteAttributeString("colspan", XmlConvert.ToString(columnSpan));
        }

        _writer.WriteString(value);
        _writer.WriteFullEndElement();
    }

    private void WriteTableRowHeader(string value)
    {
        _writer.WriteElementString("th", value);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _writer.Dispose();
            }

            _disposedValue = true;
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
