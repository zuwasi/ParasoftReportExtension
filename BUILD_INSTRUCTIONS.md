# Build Instructions

## 🛠️ Building the Extension

### Prerequisites

1. **Visual Studio 2022** (Community, Professional, or Enterprise)
2. **Visual Studio SDK** (included with Visual Studio 2022)
3. **.NET Framework 4.8 Developer Pack**
4. **Git** (for source control)

### Step-by-Step Build Process

#### 1. Clone the Repository

```bash
git clone https://github.com/zuwasi/ParasoftReportExtension.git
cd ParasoftReportExtension
```

#### 2. Open in Visual Studio

1. Open **Visual Studio 2022**
2. Open the solution file: `ParasoftReportExtension.sln`
3. Wait for NuGet packages to restore automatically

#### 3. Build the Extension

**Option A: Using Visual Studio GUI**
1. Select **Build** → **Build Solution** (Ctrl+Shift+B)
2. Choose **Release** configuration for production builds
3. The VSIX file will be created in `bin\Release\` folder

**Option B: Using Command Line**
```bash
# Build in Release mode
devenv ParasoftReportExtension.sln /build Release
```

#### 4. Locate the VSIX File

After successful build, find the installable package:
- **Path**: `bin\Release\ParasoftReportExtension.vsix`
- **Size**: ~50-100 KB (approximately)

### 🧪 Testing the Extension

#### 1. Install in Experimental Instance

1. Press **F5** in Visual Studio to launch the experimental instance
2. This creates a separate VS environment for testing
3. The extension will be automatically loaded

#### 2. Test Functionality

1. In the experimental instance:
   - Go to **Tools** → **Load Parasoft C++TEST Report**
   - Test with sample SARIF and HTML files
   - Verify Error List integration works

#### 3. Create Test Reports

**Sample SARIF file** (`test.sarif`):
```json
{
  "version": "2.1.0",
  "runs": [
    {
      "tool": {
        "driver": {
          "name": "Parasoft C++TEST"
        }
      },
      "results": [
        {
          "ruleId": "TEST-001",
          "message": {
            "text": "Test violation message"
          },
          "level": "warning",
          "locations": [
            {
              "physicalLocation": {
                "artifactLocation": {
                  "uri": "test.cpp"
                },
                "region": {
                  "startLine": 10,
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

**Sample HTML file** (`test.html`):
```html
<!DOCTYPE html>
<html>
<head><title>Test Report</title></head>
<body>
<table>
  <tr>
    <th>File</th>
    <th>Line</th>
    <th>Rule</th>
    <th>Message</th>
  </tr>
  <tr>
    <td>test.cpp</td>
    <td>10</td>
    <td>TEST-001</td>
    <td>Test violation message</td>
  </tr>
</table>
</body>
</html>
```

### 📦 Creating a Release Build

#### 1. Update Version Information

Edit `source.extension.vsixmanifest`:
```xml
<Identity Id="ParasoftReportExtension.12345678-1234-1234-1234-123456789012" 
          Version="1.0.0" 
          Language="en-US" 
          Publisher="Parasoft Report Extension" />
```

#### 2. Build Release Configuration

1. Set configuration to **Release**
2. Build the solution
3. Verify no warnings or errors

#### 3. Test the Release Build

1. Close all Visual Studio instances
2. Install the VSIX file: `bin\Release\ParasoftReportExtension.vsix`
3. Test in a fresh Visual Studio instance

### 🔧 Troubleshooting Build Issues

#### Common Build Errors

**Error: "Microsoft.VSSDK.BuildTools.targets not found"**
- Solution: Install Visual Studio SDK component
- Go to **Tools** → **Get Tools and Features** → **Individual components** → **Visual Studio SDK**

**Error: "Could not resolve assembly references"**
- Solution: Restore NuGet packages
- Right-click solution → **Restore NuGet Packages**

**Error: "VSIX manifest validation failed"**
- Solution: Check `source.extension.vsixmanifest` for syntax errors
- Ensure all required fields are filled

#### Package Reference Issues

If you encounter NuGet package issues:

1. **Clean and Rebuild**:
   ```bash
   # Delete bin and obj folders
   rm -rf bin obj
   # Rebuild
   devenv ParasoftReportExtension.sln /rebuild Release
   ```

2. **Update Package References**:
   - Right-click solution → **Manage NuGet Packages for Solution**
   - Check for updates to:
     - Microsoft.VisualStudio.SDK
     - Microsoft.VSSDK.BuildTools
     - HtmlAgilityPack
     - Newtonsoft.Json

### 📋 Build Checklist

Before releasing:

- [ ] ✅ Extension builds without errors
- [ ] ✅ Extension builds without warnings
- [ ] ✅ VSIX file is created successfully
- [ ] ✅ Extension installs correctly
- [ ] ✅ Tools menu shows the command
- [ ] ✅ SARIF parsing works correctly
- [ ] ✅ HTML parsing works correctly
- [ ] ✅ Error List integration works
- [ ] ✅ File navigation works
- [ ] ✅ Error handling works properly
- [ ] ✅ Version information is correct

### 🚀 Deployment

1. **GitHub Release**:
   - Create a new release on GitHub
   - Upload the VSIX file
   - Include release notes

2. **Visual Studio Marketplace** (Optional):
   - Upload to Visual Studio Marketplace
   - Follow Microsoft's publishing guidelines

### 📝 Build Environment Details

**Tested with:**
- Visual Studio 2022 Version 17.8.0
- Windows 11 Pro (Build 22631)
- .NET Framework 4.8
- Visual Studio SDK 17.0.32112.339

**Dependencies:**
- Microsoft.VisualStudio.SDK 17.0.32112.339
- Microsoft.VSSDK.BuildTools 17.0.5232
- HtmlAgilityPack 1.11.54
- Newtonsoft.Json 13.0.3
