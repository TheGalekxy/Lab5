using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;

namespace Lab5.Controllers
{
    public class BooksController : Controller
    {
        public IActionResult Index()
        {
            IList<Models.Book> bookList = new List<Models.Book>();

            //load books.xml
            string path = Request.PathBase + "App_Data/books.xml";
            XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                doc.Load(path);
                XmlNodeList books = doc.GetElementsByTagName("books");

                foreach (XmlElement p in books)
                {
                    Models.Book book = new Models.Book();
                    // book.Job = p.GetAttribute("job");
                    book.Id = Int32.Parse(p.GetElementsByTagName("id")[0].InnerText);
                    book.FirstName = p.GetElementsByTagName("firstname")[0].InnerText;
                    book.MiddleName = p.GetElementsByTagName("middlename")[0].InnerText;
                    book.LastName = p.GetElementsByTagName("lastname")[0].InnerText;
                    book.Title = p.GetElementsByTagName("title")[0].InnerText;


                    bookList.Add(book);
                }
            }

            return View(bookList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var book = new Models.Book();
            return View(book);
        }

        [HttpPost]
        public IActionResult Create(Models.Book p)
        {
            //load books.xml
            string path = Request.PathBase + "App_Data/books.xml";
            XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                //if file exists, just load it and create new book
                doc.Load(path);

                //create a new person
                XmlElement book = _CreateBookElement(doc, p);

                //get the root element
                doc.DocumentElement.AppendChild(book);

            }
            else
            {
                //file doesn't exist, so create and create new book
                XmlNode dec = doc.CreateXmlDeclaration("1.0", "utf-8", "");
                doc.AppendChild(dec);
                XmlNode root = doc.CreateElement("book");

                //create a new person
                XmlElement book = _CreateBookElement(doc, p);
                root.AppendChild(book);
                doc.AppendChild(root);
            }
            doc.Save(path);

            return View();
        }

        private XmlElement _CreateBookElement(XmlDocument doc, Models.Book newBook)
        {
            XmlElement book = doc.CreateElement("book");
            // XmlAttribute job = doc.CreateAttribute("job");
            // job.Value = newPerson.Job;
            // person.Attributes.Append(job);

            XmlNode Title = doc.CreateElement("Title");
            Title.InnerText = newBook.Title;

            XmlNode Author = doc.CreateElement("author");

            XmlNode firstname = doc.CreateElement("firstname");
            firstname.InnerText = newBook.FirstName;
            XmlNode middlename = doc.CreateElement("middlename");
            middlename.InnerText = newBook.MiddleName;
            XmlNode lastname = doc.CreateElement("lastname");
            lastname.InnerText = newBook.LastName;

            XmlNodeList id = doc.GetElementsByTagName("id");

            XmlNode Id = doc.CreateElement("id");

            foreach (XmlElement p in id)
            {
                //Debug.WriteLine((Int32.Parse(p.InnerText)+1).ToString());
                if((Int32.Parse(p.InnerText) + 1) < 10)
                 {
                    Id.InnerText = ("000" + (Int32.Parse(p.InnerText) + 1).ToString());
                } else if ((Int32.Parse(p.InnerText) + 1) >= 10 && (Int32.Parse(p.InnerText) + 1) < 100) {
                    Id.InnerText = ("00" + (Int32.Parse(p.InnerText) + 1).ToString());
                } else if ((Int32.Parse(p.InnerText) + 1) >= 100 && (Int32.Parse(p.InnerText) + 1) < 1000)
                {
                    Id.InnerText = ("0" + (Int32.Parse(p.InnerText) + 1).ToString());
                } else
                {
                    Id.InnerText = (Int32.Parse(p.InnerText) + 1).ToString();
                }
            }

            Author.AppendChild(firstname);
            Author.AppendChild(middlename);
            Author.AppendChild(lastname);

            book.AppendChild(Id);
            book.AppendChild(Title);
            book.AppendChild(Author);

            return book;
        }
    }
}
