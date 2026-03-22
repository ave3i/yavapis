## Setup Guide

### 1. Download Dependencies

* Download **Dependencies.zip**
* Extract it into your executor’s folder
* Make sure you already created the API (explained below)

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
6. Click **Create**

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
4. Paste the contents of:

   ```
   API.cs
   ```

---

### 5. Build the Project

⚠️ **IMPORTANT:** Set:

* Configuration → **Debug**
* Platform → **x64**

Then:

* Go to **Build → Build Solution**

You should get a `.dll` file like:

```
C:\path\to\your\project\bin\Debug\yourapi.dll
```

---

## 6. Adding the DLL to Your Executor

Now that you have your compiled API, you need to **reference it in your executor project**.

### Step 1: Copy the DLL

* Copy your compiled `.dll` file
* Paste it into your executor project folder (recommended: create a folder named `libs` or `dependencies`)

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

At the top of your code file:

```csharp
using YourAPINamespace;
```

*(Replace with whatever namespace is inside API.cs)*

---

## 8. Example Usage (WinForms)

Inside a button click:

```csharp
private void button1_Click(object sender, EventArgs e)
{
    var api = new API();

    api.Attach(); // Example function
    api.Execute("print('Hello from API')");
}
```

---

## 9. Example Usage (WPF)

Inside a button handler:

```csharp
private void Execute_Click(object sender, RoutedEventArgs e)
{
    var api = new API();

    api.Attach();
    api.Execute("print('Hello from API')");
}
```

---

## 10. Making Sure It Works

If things don’t work, check:

* ✅ Both projects use **x64**
* ✅ The DLL is **built in Debug x64**
* ✅ The reference is added correctly
* ✅ All dependencies are in the same folder
* ✅ No missing `.dll` errors on run

---

## 11. Optional: Auto Copy DLL

To avoid manual copying:

1. Right-click your API project → **Properties**
2. Go to **Build Events**
3. Add a post-build event:

```bat
copy "$(TargetPath)" "C:\path\to\your\executor\libs\"
```

---

## Final Notes

* Your executor is now connected to your custom API
* You can expand the API with more functions and call them from your UI
* A video tutorial will also be made for easier understanding

---
