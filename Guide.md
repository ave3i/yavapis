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
6. Accept any administrator permissions if prompted

---

### 3. Create a New Project

1. Open Visual Studio
2. Click **“Create a new project”**

   * You can also press `Alt + S` and search
3. Search for:
   **Class Library (.NET Framework)**
4. Select it and click **Next**
5. Choose:

   * A project name
   * (Optional) Change the project location
6. Click **Create**

---

### 4. Edit the Project Files

1. Once the project loads, locate the **Solution Explorer**

   * If you don’t see it:

     * Go to **View → Solution Explorer**
2. Find a file named something like:

   ```
   Class1.cs
   ```
3. Open it
4. Press:

   * `Ctrl + A` → select all
   * `Backspace` → delete everything
5. Paste the contents of:

   ```
   API.cs
   ```

   (from your provided GitHub link)

---

### 5. Build the Project

⚠️ **Important:**
Set the build configuration to:

* **Debug**
* **x64**

Then:

1. Go to the top menu
2. Click **Build → Build Solution**

---

### 6. Expected Output

If everything works, you should see something like:

```
1>------ Build All started: Project: YourProject, Configuration: Debug x64 ------
1>  YourProject -> C:\path\to\your\dll\yourapi.dll
========== Build All: 1 succeeded, 0 failed, 0 skipped ==========
========== Build completed ==========
```

---

### Notes

* The compiled `.dll` file is your final API output
* Make sure all steps are followed exactly, especially the **x64 Debug configuration**

---

A video version of this guide will also be made for easier understanding.
