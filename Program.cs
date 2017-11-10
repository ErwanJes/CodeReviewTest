using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
  public class LocalNotebook
  {
    private string localFilePath;
    private bool error = false;
    public string content;

    public LocalNotebook(string localFilePath)
    {
      this.localFilePath = localFilePath;
    }

    public string load()
    {
      if (localFilePath != null && localFilePath != "")
            {
        try
        {
          content = File.ReadAllText(localFilePath);
          return content;
        }
        catch (Exception)
        {
        }
      }
      error = true;
      return null;
    }

    public int loadedState()
    {
      // not loaded
      if (content == null)
      {
        return -1;
      }
      else if (error)
      {
        return 0;
      }
      // ok
      else
      {
        return 1;
      }
    }

    public bool canBeLoaded(string path)
    {
      try
      {
        string trimmedPath = path.Trim();
        if (trimmedPath.Contains("\\"))
        {
          string fullPath = Path.GetFullPath(trimmedPath);
          return true;
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine("error");
      }
      return false;
    }
  }

  public class CloudNotebook
  {
    private string fileUrl;
    private bool error = false;
    public string content;

    public CloudNotebook(string fileUrl)
    {
      this.fileUrl = fileUrl;
    }

    public string load()
    {
      if (fileUrl != null && fileUrl != "")
      {
     WebClient client = new WebClient();
      content = client.DownloadString(fileUrl);
      return content;
      }
      error = true;
      return null;
    }

    public int loadedState()
    {
      // not loaded
      if (content == null)
      {
        return -1;
      }
      else if (error)
      {
        return 0;
      }
      // ok
      else
      {
        return 1;
      }
    }
  }

  public class Storage
  {
    public static void save(string key, string content)
    {
      // `content` is empty by default
      if (content == null)
      {
        content = "";
      }
      //... save
    }
  }

  class Program
  {
    /// <summary>
    /// Try to load the content of `args`.
    /// </summary>
    /// <param name="args">local file path or url of text files</param>
    static void Main(string[] args)
    {
      LoadAllNotebooks(args);
    }

    /// <summary>
    /// Try to load the content of each file path which can be local file path or web url.
    /// </summary>
    /// <param name="filePathes"></param>
    static void LoadAllNotebooks(string[] filePathes)
    {
      for (int i = 0; i < filePathes.Count(); i++)
      {
        string filePath = filePathes[i];
        CloudNotebook cloudN = null;
        LocalNotebook ln = new LocalNotebook(filePath);
        bool canBeLoaded = ln.canBeLoaded(filePath);
        if (canBeLoaded)
        {
          ln.load();
          if (ln.loadedState() == 1)
          {
            Storage.save(filePath, ln.content);
          }
        }
        else
        {
          cloudN = new CloudNotebook(filePath);
          cloudN.load();
          if (cloudN.loadedState() == 1)
          {
            Storage.save(filePath, cloudN.content);
          }
        }

        // save the first notebook
        if (i == 0)
        {
          Storage.save("firstNotebook", filePath);
        }
      }
    }
  }
}
