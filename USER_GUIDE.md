# User Guide

## 🚀 Getting Started

After installing the extension, you can start loading Parasoft C++TEST reports into Visual Studio's Error List.

## 📂 Loading Reports

### Step 1: Access the Tool

1. Open Visual Studio 2022
2. Go to **Tools** menu
3. Click **"Load Parasoft C++TEST Report"**

### Step 2: Select Report File

1. A file dialog will open
2. Navigate to your report file location
3. Select your report file:
   - **SARIF files**: `*.sarif`
   - **HTML files**: `*.html` or `*.htm`
4. Click **Open**

### Step 3: View Results

- The extension will parse the report and load violations into the **Error List** pane
- If the Error List isn't visible, go to **View** → **Error List**

## 📊 Understanding the Results

### Error List Columns

| Column | Description |
|--------|-------------|
| **Category** | Always shows "Parasoft C++TEST" |
| **Description** | The violation message from the report |
| **File** | Source file where the violation was found |
| **Line** | Line number of the violation |
| **Project** | Current project context |

### Severity Levels

| Level | Icon | Description |
|-------|------|-------------|
| **Error** | ❌ | Critical violations that should be fixed |
| **Warning** | ⚠️ | Important violations that may cause issues |
| **Information** | ℹ️ | Informational messages and suggestions |

## 🎯 Navigating to Source Code

### Double-Click Navigation

1. **Double-click** any error in the Error List
2. Visual Studio will automatically:
   - Open the source file
   - Navigate to the exact line and column
   - Highlight the problematic code

### Requirements for Navigation

- Source file must exist at the path specified in the report
- File must be accessible from your current workspace
- Visual Studio must be able to open the file type

## 📋 Supported Report Formats

### SARIF Reports

**Best support** - Most comprehensive parsing

```json
{
  "version": "2.1.0",
  "runs": [
    {
      "tool": {
        "driver": {
          "name": "Parasoft C++TEST",
          "rules": [...]
        }
      },
      "results": [
        {
          "ruleId": "BD-PB-CC",
          "message": {
            "text": "Avoid using 'goto' statements"
          },
          "locations": [
            {
              "physicalLocation": {
                "artifactLocation": {
                  "uri": "src/main.cpp"
                },
                "region": {
                  "startLine": 42,
                  "startColumn": 5
                }
              }
            }
          ]
        }
      ]
    }
  ]
}
```

### HTML Reports

**Good support** - Flexible parsing for various formats

#### Table-Based Reports
```html
<table>
  <tr>
    <th>File</th>
    <th>Line</th>
    <th>Rule</th>
    <th>Message</th>
  </tr>
  <tr>
    <td>src/main.cpp</td>
    <td>42</td>
    <td>BD-PB-CC</td>
    <td>Avoid using 'goto' statements</td>
  </tr>
</table>
```

#### List-Based Reports
```html
<div class="violation">
  <div class="file">src/main.cpp</div>
  <div class="line">42</div>
  <div class="rule">BD-PB-CC</div>
  <div class="message">Avoid using 'goto' statements</div>
</div>
```

## 🔧 Advanced Usage

### Loading Multiple Reports

1. Load one report at a time
2. Previous results are cleared when loading a new report
3. To compare reports, use different Visual Studio instances

### Filtering Results

Use the Error List's built-in filtering:

1. **Filter by severity**: Click the Error/Warning/Message buttons
2. **Filter by text**: Use the search box at the top
3. **Filter by project**: Use the project dropdown

### Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| **F8** | Go to next error |
| **Shift+F8** | Go to previous error |
| **Ctrl+\\, E** | Open Error List |
| **Enter** | Navigate to selected error |

## 🎨 Customization

### Error List Appearance

Customize the Error List in **Tools** → **Options** → **Environment** → **Fonts and Colors**:

- Search for "Error List"
- Modify colors for different severity levels
- Adjust font size and family

## 🔍 Troubleshooting

### Common Issues

#### "No errors found in the report file"

**Possible causes:**
- Report file is empty or corrupted
- Report format is not supported
- File contains no violations

**Solutions:**
1. Verify the report file opens correctly in a text editor
2. Check that the file contains actual violations
3. Ensure the file format matches supported formats

#### "Error processing file"

**Possible causes:**
- File is corrupted or incomplete
- Unsupported file format
- File access permissions

**Solutions:**
1. Try with a different report file
2. Check file permissions
3. Verify file is not locked by another process

#### Navigation not working

**Possible causes:**
- Source file path in report is incorrect
- File has been moved or deleted
- Relative paths not resolved correctly

**Solutions:**
1. Verify source files exist at reported locations
2. Check that paths in the report are correct
3. Ensure source files are accessible from Visual Studio

### Getting Help

1. **Check the Error List** for detailed error messages
2. **View Activity Log**: Start Visual Studio with `devenv /log`
3. **Report issues** on [GitHub](https://github.com/zuwasi/ParasoftReportExtension/issues)

## 📈 Best Practices

### Report Generation

1. **Use absolute paths** in reports when possible
2. **Ensure source files are accessible** from the development machine
3. **Use SARIF format** for best results and metadata preservation

### Workflow Integration

1. **Generate reports** as part of your CI/CD pipeline
2. **Review violations** before code commits
3. **Use Error List filtering** to focus on specific issues
4. **Double-click navigation** to quickly jump to problems

## 🔄 Updates and Support

- **Check for updates** regularly on GitHub releases
- **Report bugs** or request features via GitHub issues
- **Contribute** to the project if you find improvements

## 📚 Related Resources

- [Parasoft C++TEST Documentation](https://docs.parasoft.com/display/CPPTEST)
- [SARIF Specification](https://docs.oasis-open.org/sarif/sarif/v2.1.0/sarif-v2.1.0.html)
- [Visual Studio Error List](https://docs.microsoft.com/en-us/visualstudio/ide/find-and-fix-code-errors)
