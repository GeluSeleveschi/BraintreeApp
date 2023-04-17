using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class BookPurchaseVM: Book
    {
        public string Nonce { get; set; }
    }
}
