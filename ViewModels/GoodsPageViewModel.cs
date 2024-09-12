using AvaloniaApplication2.Models;
using AvaloniaApplication2.Views;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using Spire.Doc;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication2.ViewModels
{
    public class GoodsPageViewModel : ViewModelBase
    {
        private IEnumerable<Good> goods = new List<Good>();
        public IEnumerable<Good> Goods
        {
            get => goods;
            set => this.RaiseAndSetIfChanged(ref goods, value);
        }
        private Good selectedGood = new Good();
        public Good SelectedGood
        {
            get => selectedGood;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedGood, value);
            }
        }
        public GoodsPageViewModel()
        {
            Update();
        }
        public void Update()
        {
            db = new DbIs615Context();
            db.Goods.Load();
            Goods = db.Goods.Local.ToObservableCollection();
        }
        private DbIs615Context db = new DbIs615Context();
        public void Add()
        {
            var _good = new Good();
            db.Goods.Local.Add(_good);
            Goods = db.Goods.Local.ToObservableCollection();
            SelectedGood = _good;
        }
        public void Remove()
        {
            db.Goods.Local.Remove(SelectedGood);
            Goods = db.Goods.Local.ToObservableCollection();
        }
         public void Save()   
        {

            db.SaveChanges();
            Update();

            
        }
        public void Export()
        {
            Workbook workbook = new Workbook();
            workbook.Worksheets.Clear();
            Worksheet sheet = workbook.CreateEmptySheet();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ИД");
            dataTable.Columns.Add("НАЗВАНИЕ");
            dataTable.Columns.Add("ЦЕНА");
            foreach(var item in Goods)
            {

                if (SelectedGood!.Id != null)
                if (SelectedGood!.Id == item.Id) { continue; }
                dataTable.Rows.Add(item.Id, item.Name, item.Price);
            }
            sheet.InsertDataTable(dataTable, true, 1,1);
            workbook.SaveToFile("export.xlsx", ExcelVersion.Version2007);
            string full = Environment.CurrentDirectory + "\\export.xlsx";
            Process.Start(new ProcessStartInfo
            {
                FileName = full,
                UseShellExecute = true,
            });
        }
        public void GetCard()
        {
            Document doc = new Document("Templates\\card.doc");
            doc.Replace("[name]",SelectedGood.Name,true,true);
            doc.Replace("[price]", SelectedGood.Price.ToString(), true, true);
            doc.Replace("[date]", DateTime.Now.ToShortDateString(),true, true);
            doc.SaveToFile("card.doc", Spire.Doc.FileFormat.Docm2019);
            Process.Start(new ProcessStartInfo
            {
                FileName = "card.doc",
                UseShellExecute = true,
            });

        }
    }
}