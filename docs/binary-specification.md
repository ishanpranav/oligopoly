# Binary specification
## Description
This is the specification for the fast, lightweight binary serialization format used to store _machine-readable_ Oligopoly data files. This specialized, raw format allows a high degree of flexibility while avoiding the serialization and storage overhead of structured data. It is both minimalistic and portable, although it requires more code, testing, and maintenance and is not generalizable.
## 
Board
<table>
  <thead>
    <tr>
      <th>Byte</th>
      <th>Type</th>
      <th>Value</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>0</td>
      <td>Byte</td>
      <td>
        <code>228</code>
      </td>
      <td>Specifies the format byte.</td>
    </tr>
    <tr>
      <td>1</td>
      <td>Byte</td>
      <td>
        <code>46</code>
      </td>
      <td>Specifies the version byte.</td>
    </tr>
    <tr>
      <td>2</td>
      <td rowspan="4">Integer</td>
      <td></td>
      <td rowspan="4">Specifies the number of records in the succeeding collection.</td>
    </tr>
    <tr>
      <td>3</td>
    </tr>
    <tr>
      <td>4</td>
    </tr>
    <tr>
      <td>5</td>
    </tr>
    <tr>
      <td>⋮</td>
      <td>
        <a href="#Square">Square[]</a>
      </td>
      <td></td>
      <td>A collection of data models repeated as many times as specified by the preceding integer value.</td>
    </tr>
    <tr>
      <td>6</td>
      <td rowspan="4">Integer</td>
      <td></td>
      <td rowspan="4">Specifies the number of records in the succeeding collection.</td>
    </tr>
    <tr>
      <td>7</td>
    </tr>
    <tr>
      <td>8</td>
    </tr>
    <tr>
      <td>9</td>
    </tr>
    <tr>
      <td>⋮</td>
      <td>
        <a href="#Group">Group[]</a>
      </td>
      <td></td>
      <td>A collection of data models repeated as many times as specified by the preceding integer value.</td>
    </tr>
  </tbody>
</table>

## 
Group
<table>
  <thead>
    <tr>
      <th>Byte</th>
      <th>Type</th>
      <th>Value</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>0</td>
      <td>Byte</td>
      <td>
        <code>228</code>
      </td>
      <td>Specifies the format byte.</td>
    </tr>
    <tr>
      <td>1</td>
      <td>Byte</td>
      <td>
        <code>46</code>
      </td>
      <td>Specifies the version byte.</td>
    </tr>
  </tbody>
</table>

## 
StartSquare
<table>
  <thead>
    <tr>
      <th>Byte</th>
      <th>Type</th>
      <th>Value</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>0</td>
      <td>Byte</td>
      <td>
        <code>228</code>
      </td>
      <td>Specifies the format byte.</td>
    </tr>
    <tr>
      <td>1</td>
      <td>Byte</td>
      <td>
        <code>46</code>
      </td>
      <td>Specifies the version byte.</td>
    </tr>
    <tr>
      <td rowspan="4">2</td>
      <td rowspan="4">Square Type</td>
      <td>
        <code>0</code>
      </td>
      <td>Start</td>
    </tr>
    <tr>
      <td>
        <code>1</code>
      </td>
      <td>Street</td>
    </tr>
    <tr>
      <td>
        <code>2</code>
      </td>
      <td>Chance</td>
    </tr>
    <tr>
      <td>
        <code>3</code>
      </td>
      <td>Community</td>
    </tr>
  </tbody>
</table>

## 
StreetSquare
<table>
  <thead>
    <tr>
      <th>Byte</th>
      <th>Type</th>
      <th>Value</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>0</td>
      <td>Byte</td>
      <td>
        <code>228</code>
      </td>
      <td>Specifies the format byte.</td>
    </tr>
    <tr>
      <td>1</td>
      <td>Byte</td>
      <td>
        <code>46</code>
      </td>
      <td>Specifies the version byte.</td>
    </tr>
    <tr>
      <td rowspan="4">2</td>
      <td rowspan="4">Square Type</td>
      <td>
        <code>0</code>
      </td>
      <td>Start</td>
    </tr>
    <tr>
      <td>
        <code>1</code>
      </td>
      <td>Street</td>
    </tr>
    <tr>
      <td>
        <code>2</code>
      </td>
      <td>Chance</td>
    </tr>
    <tr>
      <td>
        <code>3</code>
      </td>
      <td>Community</td>
    </tr>
    <tr>
      <td>⋮</td>
      <td>String</td>
      <td></td>
      <td></td>
    </tr>
    <tr>
      <td>3</td>
      <td rowspan="4">Integer</td>
      <td></td>
      <td></td>
    </tr>
    <tr>
      <td>4</td>
    </tr>
    <tr>
      <td>5</td>
    </tr>
    <tr>
      <td>6</td>
    </tr>
    <tr>
      <td>7</td>
      <td rowspan="4">Integer</td>
      <td></td>
      <td></td>
    </tr>
    <tr>
      <td>8</td>
    </tr>
    <tr>
      <td>9</td>
    </tr>
    <tr>
      <td>10</td>
    </tr>
    <tr>
      <td>11</td>
      <td rowspan="4">Integer</td>
      <td></td>
      <td></td>
    </tr>
    <tr>
      <td>12</td>
    </tr>
    <tr>
      <td>13</td>
    </tr>
    <tr>
      <td>14</td>
    </tr>
    <tr>
      <td>15</td>
      <td rowspan="4">Integer</td>
      <td></td>
      <td></td>
    </tr>
    <tr>
      <td>16</td>
    </tr>
    <tr>
      <td>17</td>
    </tr>
    <tr>
      <td>18</td>
    </tr>
  </tbody>
</table>

