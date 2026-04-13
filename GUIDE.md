# Setup Guide

---

## 2. Install Visual Studio

Download **Visual Studio 2022 or 2026**, then open the **Visual Studio Installer**.

1. Click **Modify** on your installed version
2. Select **.NET desktop development**
3. Click **Install** and accept any administrator prompts

---

## 3. Create a New Project

1. Open Visual Studio and click **Create a new project**
2. Search for and select **Class Library (.NET Framework)**
3. Click **Next** and fill in your project name and location
4. ⚠️ Make sure the framework is set to **.NET 4.8**
5. Click **Create**

---

## 4. Add the API Code

1. Open **Solution Explorer** — if it's not visible, go to **View → Solution Explorer**
2. Open `Class1.cs`
3. Select all with `Ctrl + A`, then delete with `Backspace`
4. Paste the contents of [`API.cs`](https://github.com/ave3i/yavapis/blob/main/API.cs)
5. Replace every instance of `yourAPI` with your chosen API/project name

---

## 5. Build the Project

> ⚠️ Before building, make sure these are set correctly:
> - Configuration → **Debug**
> - Platform → **x64**

**Optional — rename your DLL output:**
Go to **Project → Properties** and change the `Assembly Name` and `Default Namespace` to your chosen name.

Then build via **Build → Build Solution**. Your `.dll` will appear at:
```
C:\path\to\your\project\bin\Debug\YourAPI.dll
```

---

## 6. Adding the DLL to Your Executor

### Step 1 — Copy the DLL
Copy your compiled `.dll` and paste it into your executor project folder.

### Step 2 — Reference it in Visual Studio
1. Open your executor project (WinForms or WPF)
2. In **Solution Explorer**, right-click **References**
3. Click **Add Reference...**
4. Select **Browse**, locate your `.dll`, then click **Add → OK**

---

## 7. Using the API

At the top of your executor's `.cs` file, add:
```csharp
using yourAPI; // Replace with your actual namespace name
```

---

## 8. Example Usage

```csharp
private void button1_Click(object sender, EventArgs e)
{
    API.AttachWithAPI(pid); // PID PARAMETER IS OPTIONAL IF YOU WANT TO ATTACH TO ALL PROCESSES!
}

private void button2_Click(object sender, EventArgs e)
{
    API.Execute(RichTextBox1.Text, pid); // PID PARAMETER IS OPTIONAL IF YOU WANT TO ATTACH TO ALL PROCESSES!
}
```

---

## 9. Troubleshooting

If something isn't working, run through this checklist:

| Check | Detail |
|---|---|
| ✅ Both projects are **x64** | Mismatched platforms will cause load failures |
| ✅ DLL built in **Debug x64** | Release builds may behave differently |
| ✅ Reference added correctly | Verify it appears under References in Solution Explorer |
| ✅ All dependencies present | Keep all required `.dll` files in the same folder |
| ✅ No missing DLL errors on run | Check the output window for specific missing files |

---

## Final Notes

- You can extend the API with additional functions and call them from your UI at any time
- A video tutorial is planned for those who prefer a visual walkthrough
