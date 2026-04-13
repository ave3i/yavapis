## Setup Guide

---

### 2. Install Visual Studio

1. Download **Visual Studio 2022 or 2026**
2. Open the **Visual Studio Installer**
3. Click **Modify** on your installed version
4. Select **.NET desktop development**
5. Click **Install**
6. Accept administrator permissions if prompted

---

### 3. Create a New Project

1. Open Visual Studio
2. Click **Create a new project**
3. Search for:

   ```
   Class Library (.NET Framework)
   ```
4. Click **Next**
5. Choose:

   * Project name
   * (Optional) Project location
6. The Project must be on **.NET 4.8**!
7. Click **Create**

---

### 4. Add the API Code

1. Open **Solution Explorer**

   * If not visible: **View → Solution Explorer**
2. Open:

   ```
   Class1.cs
   ```
3. Press:

   * `Ctrl + A` → `Backspace`
4. Paste the contents of [```API.cs```](https://github.com/ave3i/yavapis/blob/main/API.cs), and replace wherever `yourAPI` 
is with your API/Project name or generally whatever you want.

---

### 5. Build the Project

⚠️ **IMPORTANT:** Set:

* Configuration → **Debug**
* Platform → **x64**

Then:

* Go to **Build → Build Solution**

Before you build, you can change the name of the dll file by going to Project → Properties → On the top 
left where it says `Assembly Name`, and change `Default Namespace` to what you named your namespace.
\nYou should get a `.dll` file in the output right below your code/ui preview like:

```
C:\path\to\your\project\bin\Debug\YourAPI.dll
```

---

## 6. Adding the DLL to Your Executor

Now that you have your compiled API, you need to **reference it in your executor project**.

### Step 1: Copy the DLL

* Copy your compiled `.dll` file
* Paste it into your executor project folder.

---

### Step 2: Add Reference in Visual Studio

1. Open your executor project (WinForms or WPF)
2. In **Solution Explorer**, right-click:

   ```
   References
   ```
3. Click:

   ```
   Add Reference...
   ```
4. Select **Browse**
5. Locate and select your `.dll`
6. Click **Add → OK**

---

## 7. Using the API in Your Executor

### Import the Namespace

Go to your `.cs` file of your exploit, and at the top of your code file add:

```csharp
using yourAPI;
```

*(Replace with whatever namespace is inside API.cs)*

---

## 8. Example Usage

Inside a button click:

```csharp
private void button1_Click(object sender, EventArgs e)
{
    API.InjectAPI();
}

private void button2_Click(object sender, EventArgs e)
{
    API.Execute(RichTextBox1.Text);
}
```

---

## 9. Troubleshooting

If things don’t work, check:

* ✅ Both projects use **x64**
* ✅ The DLL is **built in Debug x64**
* ✅ The reference is added correctly
* ✅ All dependencies are in the same folder
* ✅ No missing `.dll` errors on run

Also, if you get an error like :
`The name 'ZipFile' does not exist in the current context`

Hover over the error'ed text, and press Install `System.IO.Compression`, and you also need to install `System.IO.Compression.FileSystem`.

---

## Final Notes

* You can expand the API with more functions and call them from your UI
* A video tutorial will also be made for easier understanding

---
