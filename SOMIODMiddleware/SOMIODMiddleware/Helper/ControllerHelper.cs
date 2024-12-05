using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace SOMIODMiddleware.Helper
{
    public class ControllerHelper
    {
        public XmlNode BuildXmlNodeFromRequest(string element)
        {
           var streamPostData = new StreamReader(HttpContext.Current.Request.InputStream);// le o request
           string xmlData = streamPostData.ReadToEnd();

           XmlDocument xmlDocument = new XmlDocument(); // cria um novo documento XML
           xmlDocument.LoadXml(xmlData);

            if (ValidateXmlObject(xmlDocument))
            {
                return new XmlDocument();
            }

            if (element != null)
            {
                return xmlDocument.SelectSingleNode(element);
            }
            return new XmlDocument();


        }
        public Boolean ValidateXmlObject(XmlDocument xmlDocument)
        {
            return xmlDocument == null;
        }
    }
}