using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Globalization;
using System.IO;
using Microsoft.Win32;

namespace Metaproject.Reg
{
 
    public class Settings : IDisposable
    {
     
        public enum Domain { Machine, MachineReadOnly, User };

     
        private RegistryKey parentKey;
        private RegistryKey key;
        private string path;
        private List<string> names;

     
     
        public Settings(Domain domain, string path)
        {
            InitializeSettings(domain, path, false);
        }

        public Settings(Domain domain, string path, bool dont_add_prefix = true)
        {
            InitializeSettings(domain, path, dont_add_prefix);
        }

        private void InitializeSettings(Domain domain, string path, bool dont_add_prefix = true)
        {
            if (dont_add_prefix)
            {
                this.path = path;
            }
            else
            {
                this.path = String.Format("Software\\Metaproject\\{0}", path);
            }
            switch (domain)
            {
                case Domain.Machine:
                    parentKey = GetParentKey(RegistryHive.LocalMachine);
                    key = parentKey.CreateSubKey(this.path);
                    break;
                case Domain.MachineReadOnly:
                    parentKey = GetParentKey(RegistryHive.LocalMachine);
                    key = parentKey.CreateSubKey(this.path, RegistryKeyPermissionCheck.ReadSubTree);
                    break;
                case Domain.User:
                    parentKey = GetParentKey(RegistryHive.CurrentUser);
                    key = parentKey.CreateSubKey(this.path);
                    break;
            }

            names = new List<string>();
            foreach (string name in key.GetValueNames())
                names.Add(name.ToUpper());
        }

        private RegistryKey GetParentKey(RegistryHive registryHive)
        {

            if (registryHive == RegistryHive.LocalMachine)
                return Registry.LocalMachine;

            return Registry.CurrentUser;
        
        }


     
        public void Dispose()
        {
            key.Close();
            parentKey.Close();
        }

    

  
        public bool HasValue(string name)
        {
            return names.Contains(name.ToUpper());
        }

    
        public void SetInt16(string name, Int16 value)
        {
            key.SetValue(name, value, RegistryValueKind.DWord);
            names.Add(name.ToUpper());
        }


        public void SetInt32(string name, Int32 value)
        {
            key.SetValue(name, value, RegistryValueKind.DWord);
            names.Add(name.ToUpper());
        }

   
        public void SetInt64(string name, Int64 value)
        {
            key.SetValue(name, value, RegistryValueKind.QWord);
            names.Add(name.ToUpper());
        }

      
     
        public void SetFloat(string name, float value)
        {
            SetString(name, value.ToString());
        }

       
        public void SetDouble(string name, double value)
        {
            SetString(name, value.ToString());
        }

  
        public void SetString(string name, string value)
        {
            key.SetValue(name, value, RegistryValueKind.String);
            names.Add(name.ToUpper());
        }

    
        public void SetMultiString(string name, string[] value)
        {
            key.SetValue(name, value, RegistryValueKind.MultiString);
            names.Add(name.ToUpper());
        }


     
        public void SetBoolean(string name, bool value)
        {
            SetInt16(name, (value) ? (short)(1) : (short)(0));
        }

       
        public void SetRectangle(string name, Rectangle value)
        {
            SetString(name, String.Format("{0},{1},{2},{3},{4}", 1, value.X, value.Y, value.Width, value.Height));
        }

    
        public void SetSize(string name, Size value)
        {
            SetString(name, String.Format("{0},{1}", value.Width, value.Height));
        }

      
        public void SetFont(string name, Font value)
        {
            SetString(name, String.Format("{0},{1},{2},{3}",
                1,
                value.FontFamily.Name,
                value.Size.ToString(CultureInfo.InvariantCulture),
                (int)(value.Style)));
        }

     
        public void SetValue(string name, object value)
        {
            key.SetValue(name, value);
            names.Add(name.ToUpper());
        }

     
        public void DeleteValue(string name)
        {
            key.DeleteValue(name, false);
            names.Remove(name.ToUpper());
        }

       
        public void SetGuid(string name, Guid value)
        {
            key.SetValue(name, value, RegistryValueKind.String);
            names.Add(name.ToUpper());
        }

       
        public void SetColor(string name, Color value)
        {
            key.SetValue(name, value.ToArgb(), RegistryValueKind.DWord);
        }

    
        public Int16 GetInt16(string name)
        {
            return GetInt16(name, (Int16)(0));
        }

       
        public Int16 GetInt16(string name, Int16 def)
        {
            if (HasValue(name))
            {
                if (key.GetValueKind(name) == RegistryValueKind.DWord)
                {
                    // Wartość typu Int32 można rzutować do Int16, ale nie wtedy, kiedy jest zaboksowana.
                    // Dlatego najpierw musimy ją wyciągnąć z pudełka, a dopiero potem rzucać.
                    return (Int16)((Int32)(key.GetValue(name, def)));
                }
                else
                {
                    return def;
                }
            }
            else
            {
                return def;
            }
        }

       
        public Int32 GetInt32(string name)
        {
            return GetInt32(name, (Int32)(0));
        }

       
        public Int32 GetInt32(string name, Int32 def)
        {
            if (HasValue(name))
            {
                if (key.GetValueKind(name) == RegistryValueKind.DWord)
                {
                    return (Int32)(key.GetValue(name, def));
                }
                else
                {
                    return def;
                }
            }
            else
            {
                return def;
            }
        }

        
        public Int64 GetInt64(string name)
        {
            return GetInt64(name, (Int64)(0));
        }

     
        public Int64 GetInt64(string name, Int64 def)
        {
            if (HasValue(name))
            {
                if (key.GetValueKind(name) == RegistryValueKind.QWord)
                {
                    return (Int64)(key.GetValue(name, def));
                }
                else
                {
                    return def;
                }
            }
            else
            {
                return def;
            }
        }

       
        public float GetFloat(string name)
        {
            return GetFloat(name, 0.0f);
        }

     
        public float GetFloat(string name, float def)
        {
            return float.Parse(GetString(name, def.ToString()));
        }

    
        public double GetDouble(string name)
        {
            return GetDouble(name, 0.0f);
        }

        public double GetDouble(string name, double def)
        {
            return double.Parse(GetString(name, def.ToString()));
        }

      
        public bool GetBoolean(string name)
        {
            return GetBoolean(name, false);
        }

      
        public bool GetBoolean(string name, bool def)
        {
            if (HasValue(name))
            {
                if (key.GetValueKind(name) == RegistryValueKind.DWord)
                {
                    return (int)(key.GetValue(name, def)) != 0;
                }
                else
                {
                    return def;
                }
            }
            else
            {
                return def;
            }
        }

     
        public string GetString(string name)
        {
            return GetString(name, string.Empty);
        }

    
        public string GetString(string name, string def)
        {
            if (HasValue(name))
            {
                if (key.GetValueKind(name) == RegistryValueKind.String)
                {
                    return (string)(key.GetValue(name, def));
                }
                else
                {
                    return def;
                }
            }
            else
            {
                return def;
            }
        }

      
        public string[] GetMultiString(string name)
        {
            return GetMultiString(name, new string[0]);
        }

     
        public string[] GetMultiString(string name, string[] def)
        {
            if (HasValue(name))
            {
                if (key.GetValueKind(name) == RegistryValueKind.MultiString)
                {
                    return (string[])(key.GetValue(name, def));
                }
                else
                {
                    return def;
                }
            }
            else
            {
                return def;
            }
        }


     
        public Rectangle GetRectangle(string name)
        {
            return GetRectangle(name, new Rectangle());
        }

     
        public Rectangle GetRectangle(string name, Rectangle def)
        {
            if (HasValue(name))
            {
                string rectstr = GetString(name);
                if (rectstr.Length > 0)
                {
                    char[] delim = ",".ToCharArray();
                    string[] subs = rectstr.Split(delim, 5);
                    if (subs.Length == 5)
                    {
                        int version = int.Parse(subs[0]);
                        if (version == 1)
                        {
                            int x = int.Parse(subs[1]);
                            int y = int.Parse(subs[2]);
                            int w = int.Parse(subs[3]);
                            int h = int.Parse(subs[4]);
                            return new Rectangle(x, y, w, h);
                        }
                    }
                }
                return def;
            }
            else
            {
                return def;
            }
        }

      
        public Size GetSize(string name)
        {
            return GetSize(name, new Size());
        }

      
        public Size GetSize(string name, Size def)
        {
            if (HasValue(name))
            {
                string rectstr = GetString(name);
                if (rectstr.Length > 0)
                {
                    char[] delim = ",".ToCharArray();
                    string[] subs = rectstr.Split(delim, 2);
                    if (subs.Length == 2)
                    {
                        int w = int.Parse(subs[0]);
                        int h = int.Parse(subs[1]);
                        return new Size(w, h);
                    }
                }
                return def;
            }
            else
            {
                return def;
            }
        }

       
        public Font GetFont(string name)
        {
            return GetFont(name, System.Drawing.SystemFonts.DefaultFont);
        }

    
        public Font GetFont(string name, Font def)
        {
            if (HasValue(name))
            {
                string fontstr = GetString(name);
                if (fontstr.Length > 0)
                {
                    char[] delim = ",".ToCharArray();
                    string[] subs = fontstr.Split(delim, 4);
                    if (subs.Length == 4)
                    {
                        int version = int.Parse(subs[0]);
                        if (version == 1)
                        {
                            string family = subs[1];
                            float size = float.Parse(subs[2], CultureInfo.InvariantCulture);
                            FontStyle style = (FontStyle)(int.Parse(subs[3]));
                            return new Font(family, size, style);
                        }
                    }
                }
                return def;
            }
            else
            {
                return def;
            }
        }

      
        public object GetValue(string name, object def)
        {
            if (HasValue(name))
            {
                return key.GetValue(name, def);
            }
            else
            {
                return def;
            }
        }

       
        public Guid GetGuid(string name)
        {
            return GetGuid(name, Guid.Empty);
        }

       
        public Guid GetGuid(string name, Guid def)
        {
            if (HasValue(name))
            {
                if (key.GetValueKind(name) == RegistryValueKind.String)
                {
                    try
                    {
                        return new Guid(key.GetValue(name, def) as string);
                    }
                    catch
                    {
                        return def;
                    }
                }
                else
                {
                    return def;
                }
            }
            else
            {
                return def;
            }
        }

      
        public Color GetColor(string name, Color def)
        {
            if (HasValue(name))
            {
                if (key.GetValueKind(name) == RegistryValueKind.DWord)
                {
                    try
                    {
                        return Color.FromArgb((Int32)(key.GetValue(name)));
                    }
                    catch
                    {
                        return def;
                    }
                }
                else
                {
                    return def;
                }
            }
            else
            {
                return def;
            }
        }

    
        public void Clear()
        {
            parentKey.DeleteSubKeyTree(path);
            names.Clear();
            key = parentKey.CreateSubKey(path);
        }

      
    }
}