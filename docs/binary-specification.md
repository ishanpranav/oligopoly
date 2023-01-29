# Binary specification
## Description
This is the specification for the fast, lightweight binary serialization format used to store _machine-readable_ Oligopoly data files. This specialized, raw format allows a high degree of flexibility while avoiding the serialization and storage overhead of structured data. It is both minimalistic and portable, although it requires more code, testing, and maintenance and is not generalizable.
## Board
<table>
  <thead>
    <tr>
      <th>Byte</th>
      <th>Field</th>
      <th>Type</th>
      <th>Value</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td colspan="5">
        <a href="square">Square</a> Collection</td>
    </tr>
    <tr>
      <td>0</td>
      <td rowspan="4">1</td>
      <td rowspan="4">Integer</td>
      <td rowspan="4">—</td>
      <td rowspan="4">The number of elements contained in the collection.</td>
    </tr>
    <tr>
      <td>1</td>
    </tr>
    <tr>
      <td>2</td>
    </tr>
    <tr>
      <td>3</td>
    </tr>
    <tr>
      <td colspan="5">
        <a href="group">Group</a> Collection</td>
    </tr>
    <tr>
      <td>4</td>
      <td rowspan="4">2</td>
      <td rowspan="4">Integer</td>
      <td rowspan="4">—</td>
      <td rowspan="4">The number of elements contained in the collection.</td>
    </tr>
    <tr>
      <td>5</td>
    </tr>
    <tr>
      <td>6</td>
    </tr>
    <tr>
      <td>7</td>
    </tr>
  </tbody>
</table>
## Group
<table>
  <thead>
    <tr>
      <th>Byte</th>
      <th>Field</th>
      <th>Type</th>
      <th>Value</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody />
</table>
