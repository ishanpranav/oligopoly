using System.Xml;

namespace Oligopoly.Writers;

public class HtmlFragmentWriter : Writer, IDisposable
{
    private const string TableRowElement = "tr";
    private const string TableRowDataElement = "td";
    private const string RowSpanAttribute = "rowspan";

    private static XmlWriterSettings? s_settings;

    private readonly XmlWriter _writer;

    private int _byteIndex;
    private int _fieldIndex = 1;
    private bool _disposedValue;
    private string? _description;

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
        WriteTableRowHeader("Field");
        WriteTableRowHeader("Type");
        WriteTableRowHeader("Value");
        WriteTableRowHeader("Description");
        _writer.WriteEndElement();
        _writer.WriteEndElement();
        _writer.WriteStartElement("tbody");
    }

    /// <inheritdoc/>
    public override void Write(int value)
    {
        WriteTableRow("Integer", rowSpan: 4);
    }

    /// <inheritdoc/>
    public override void Write(IWritable value)
    {
        if (_byteIndex > 0)
        {
            WriteStartTableRow(value.GetType());
            WriteEndTableRow();
        }

        base.Write(value);
    }

    /// <inheritdoc/>
    public override void Write<TWritable>(IReadOnlyCollection<TWritable> value)
    {
        WriteStartTableRow(typeof(TWritable));
        _writer.WriteString(" Collection");
        WriteEndTableRow();

        _description = "The number of elements contained in the collection.";

        base.Write(value);
    }

    /// <inheritdoc/>
    public override void WriteVersion()
    {
        WriteTableRow(FormatByte);
        WriteTableRow(VersionByte);
    }

    private void WriteStartTableRow(Type type)
    {
        _writer.WriteStartElement(TableRowElement);
        _writer.WriteStartElement(TableRowDataElement);
        _writer.WriteAttributeString("colspan", XmlConvert.ToString(5));
        _writer.WriteStartElement("a");
        _writer.WriteAttributeString("href", type.Name.ToLower());
        _writer.WriteString(type.Name);
        _writer.WriteEndElement();
    }

    private void WriteEndTableRow()
    {
        _writer.WriteEndElement();
        _writer.WriteEndElement();
    }

    private void WriteTableRow(byte value)
    {
        _writer.WriteStartElement(TableRowElement);
        WriteTableRowData(_byteIndex);
        WriteTableRowData(_fieldIndex);
        WriteTableRowData("Byte");
        _writer.WriteStartElement(TableRowDataElement);
        _writer.WriteElementString("code", XmlConvert.ToString(value));
        _writer.WriteEndElement();
        WriteTableRowData(_description);
        _writer.WriteEndElement();

        _byteIndex++;
        _fieldIndex++;
        _description = null;
    }

    private void WriteTableRow(string type, int rowSpan)
    {
        string rowSpanString = XmlConvert.ToString(rowSpan);

        _writer.WriteStartElement(TableRowElement);
        WriteTableRowData(_byteIndex);
        WriteTableRowData(_fieldIndex, rowSpanString);
        WriteTableRowData(type, rowSpanString);
        _writer.WriteStartElement(TableRowDataElement);
        _writer.WriteAttributeString(RowSpanAttribute, rowSpanString);
        _writer.WriteString("—");
        _writer.WriteEndElement();
        WriteTableRowData(_description, rowSpanString);
        _writer.WriteEndElement();

        _description = null;
        _byteIndex++;
        _fieldIndex++;

        for (int i = 1; i < rowSpan; i++)
        {
            _writer.WriteStartElement(TableRowElement);
            WriteTableRowData(_byteIndex);
            _writer.WriteEndElement();

            _byteIndex++;
        }
    }

    private void WriteTableRowHeader(string value)
    {
        _writer.WriteElementString("th", value);
    }

    private void WriteTableRowData(string? value)
    {
        _writer.WriteElementString(TableRowDataElement, value);
    }

    private void WriteTableRowData(int value)
    {
        WriteTableRowData(XmlConvert.ToString(value));
    }

    private void WriteTableRowData(string? value, string rowSpan)
    {
        _writer.WriteStartElement(TableRowDataElement);
        _writer.WriteAttributeString(RowSpanAttribute, rowSpan);
        _writer.WriteString(value);
        _writer.WriteEndElement();
    }

    private void WriteTableRowData(int value, string rowSpan)
    {
        WriteTableRowData(XmlConvert.ToString(value), rowSpan);
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
