using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uzPro.Properties;


namespace uzPro.Utils
{
    public static class PDFManager
    {

        static string fg = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "Arial.TTF");
        static BaseFont fgBaseFont = BaseFont.CreateFont(fg, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        static Font fgFont = new Font(fgBaseFont, 10, Font.NORMAL, BaseColor.BLACK);


        /// <summary>
        /// Договор1
        /// </summary>
        /// <param name="pacient"></param>
        /// <returns></returns>
        public static string GetContract(Dictionary<string, string> contract, DataTable dtService)
        {
            Document document = new Document(PageSize.A4, 20, 20, 20, 20);
            string template = ReadTempaleFromFile("contract");
            string html = ReplaceText(template, contract);
            Single summa = 0;
            for (int i = 0; i < dtService.Rows.Count; i++)
            {
                html += string.Format("<tr><td>{0}</td><td>{1}</td><td colspan=5>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td></tr>", (i + 1), dtService.Rows[i][0].ToString(), dtService.Rows[i][1].ToString(), dtService.Rows[i][2].ToString(), dtService.Rows[i][3].ToString(), dtService.Rows[i][4].ToString(), dtService.Rows[i][5].ToString(), dtService.Rows[i][6].ToString());
                summa += Single.Parse(dtService.Rows[i][6].ToString().Replace(".", ","));
            }
            string sign = Properties.Settings.Default.sign;
            html += "</table></td></tr></table></div>";
            html += "<div> Итого: " + RusNumber.RurPhraseVal(Convert.ToDecimal(summa)) + " (" + RusNumber.RurPhrase(Convert.ToDecimal(summa)) + ")</div>";
            html += "<div>Услуги выполнены в полном объеме, в установленные сроки и с надлежащим качеством. Стороны претензий друг к другу не имеют.</div>";
            html += "<table  border='0'><tr><td>Заказчик<br/> ПРИНЯЛ________________</td><td>,ИСПОЛНИТЕЛЬ<br/>СДАЛ _________________" + sign + " </td></tr></table>";

            return CreateFromHtml(html, "contract.pdf", document);
        }

        private static string ReadTempaleFromFile(string name)
        {
            string fileName = string.Format(@"Templates\\{0}.txt", name);
            using (StreamReader sr = new StreamReader(fileName, Encoding.Default))
            {
                return sr.ReadToEnd();
            }
        }

        private static string ReplaceText(string ak, Dictionary<string, string> contract)
        {
            foreach (var field in contract)
            {                
                ak = ak.Replace("%" + field.Key + "%", field.Value);                
            }
            return ak;
        }


        private static string CreateFromHtml(string html, string _fileName)
        {
            Document document = new Document(PageSize.A4.Rotate(), 10, 0, 0, 0);
            return CreateFromHtml(html, _fileName, document);
        }

        private static string CreateFromHtml(string html, string _fileName, Document document)
        {
            string fileName = _fileName;
            Document _document = document;

            string arialuniTff = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
            FontFactory.Register(arialuniTff);
            StyleSheet ST = new StyleSheet();
            ST.LoadTagStyle(HtmlTags.BODY, HtmlTags.FACE, "Arial");
            ST.LoadTagStyle(HtmlTags.BODY, HtmlTags.ENCODING, BaseFont.IDENTITY_H);
            ST.LoadStyle("myBorder", "style", "font-size: 18px; text-align: center;");

            try
            {
                PdfWriter.GetInstance(_document, new FileStream(fileName, FileMode.Create));
                _document.Open();
                string htmlText = @"<?xml version=""1.0"" encoding=""UTF-8""?>                
                 <html>                              
                  <body style='font-famaly:Arial' >" + html + "</body></html>";
                List<IElement> htmlarraylist = HTMLWorker.ParseToList(new StringReader(htmlText), ST);
                for (int k = 0; k < htmlarraylist.Count; k++)
                {
                    _document.Add((IElement)htmlarraylist[k]);
                }

                _document.Close();
            }
            catch
            {
            }
            return fileName;
        }
    }
}
