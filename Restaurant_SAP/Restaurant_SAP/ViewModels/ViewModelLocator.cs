using Restaurant_SAP.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_SAP.ViewModels
{
    public class ViewModelLocator
    {
        private readonly RestauranteContext _context;

        public ViewModelLocator()
        {
            _context = new RestauranteContext();
            MesasViewModel = new MesasViewModel(_context);
            MenusViewModel = new MenusViewModel(_context);
            MapaMesasViewModel = new MapaMesasViewModel(MesasViewModel);
        }

        public MesasViewModel MesasViewModel { get; }
        public MenusViewModel MenusViewModel { get; }
        public MapaMesasViewModel MapaMesasViewModel { get; }

        public void Cleanup()
        {
            _context.Dispose();
        }

        
    }
}
