# **FolderSync - File Synchronization Tool**  
A lightweight utility that keeps a **source folder** and a **replica folder** synchronized, with logging support.  

---

## **📦 Features**  
✅ **One-way synchronization** (Source → Replica)  
✅ **Periodic sync** (configurable interval)  
✅ **Logging** (console + file)  
✅ **File comparison** (checksum-based)  
✅ **Safe deletion** (removes files in Replica not present in Source)  

---

## **🚀 How to Run**  

### **1. Prerequisites**  
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)  
- Two folders (Source and Replica)  

### **2. Command Line Usage**  
```sh
dotnet run -- <sourceFolder> <replicaFolder> <syncIntervalInSeconds> <logFilePath>
```  

#### **Example**  
```sh
dotnet run -- "C:\MyFiles\Source" "C:\MyFiles\Backup" 60 "C:\Logs\sync_log.txt"
```  
- Syncs every **60 seconds**.  
- Logs to `C:\Logs\sync_log.txt`.  

---

## **📝 Notes**  
- **First run** will copy all files from Source → Replica.  
- **Subsequent runs** only sync changes.  
- **Press `Ctrl+C`** to stop the sync process.  

---

## **Example Log Output**  
```
2024-01-01 12:00:00 - Starting folder synchronization scheduler...  
2024-01-01 12:00:00 - Copied/Updated: document.txt  
2024-01-01 12:00:00 - Deleted: old_file.txt  
```  
