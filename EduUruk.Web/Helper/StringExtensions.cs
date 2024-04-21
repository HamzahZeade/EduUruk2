using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EduUruk.Web.Helper
{
    public static class StringExtensions
    {
        public static string getColorFromFirstLetter(string letter)
        {
            string[] colors = {"text-primary bg-light-primary",
                               "text-success bg-light-success",
                               "text-info bg-light-info",
                               "text-warning bg-light-warning",
                               "text-danger bg-light-danger",
                               "text-dark bg-light-dark",
                               "text-muted bg-light",
                               "text-primary bg-light-primary",
                               "text-success bg-light-success",
                               "text-info bg-light-info"};
            try
            {
                int letterCode = Convert.ToChar(letter);
                string x = letterCode.ToString();
                string y = x.Substring(x.Count() - 1);

                return colors[Convert.ToInt32(y)];
            } catch (Exception ex) {
                return colors[6];
            }
        }
        public static string getColorFromNumber(int number)
        {
            string[] colors = {"text-primary",
                               "text-success",
                               "text-info",
                               "text-warning",
                               "text-danger",
                               "text-dark",
                               "text-muted",
                               "text-primary",
                               "text-success",
                               "text-info"};
            try
            {
                return colors[number];
            }
            catch (Exception ex)
            {
                return colors[0];
            }
        }
        public static string getIconByFileType(string fileType)
        {
            string icon = "/assets/media/svg/files/upload.svg";
            try
            {
                string[,] icons = {{"pdf", "/assets/media/svg/files/pdf.svg" },
                               { "application/pdf", "/assets/media/svg/files/pdf.svg" },
                               { "text/plain", "/assets/media/svg/files/txt.svg" },
                               { "plain", "/assets/media/svg/files/txt.svg" },
                               { "msword", "/assets/media/svg/files/doc.svg"},
                               { "vnd.openxmlformats-officedocument.wordprocessingml.document", "/assets/media/svg/files/doc.svg"},
                               { "vnd.ms-excel", "/assets/media/svg/files/xls.svg"},
                               { "vnd.openxmlformats-officedocument.spreadsheetml.sheet", "/assets/media/svg/files/xls.svg"},
                               { "vnd.ms-powerpoint", "/assets/media/svg/files/ppt.svg" },
                               { "vnd.openxmlformats-officedocument.presentationml.presentation", "/assets/media/svg/files/ppt.svg"},
                               { "jpeg", "/assets/media/svg/files/jpg.svg"},
                               { "gif", "/assets/media/svg/files/gif.svg"},
                               { "png", "/assets/media/svg/files/png.svg"},
                               { "mpeg", "/assets/media/svg/files/mp3.svg"},
                               { "mp4", "/assets/media/svg/files/mpg.svg"},
                               { "mov", "/assets/media/svg/files/mov.svg"},
                               { "x-zip-compressed", "/assets/media/svg/files/zip.svg"},
                               { "zip", "/assets/media/svg/files/zip.svg"},
                               { "vnd.rar", "/assets/media/svg/files/zip.svg"},
                               { "non", "/assets/media/svg/files/upload.svg"}};

                icon = icons[19, 1];
                for (int i = 0; i < icons.GetLength(0); i++)
                {
                    for (int j = 0; j < icons.GetLength(1); j++)
                    {
                        if (icons[i, j] == fileType.Substring(fileType.IndexOf('/') + 1))
                        {
                            icon = icons[i, j+1];
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return icon;
            }

            return icon;
        }
    }
}
