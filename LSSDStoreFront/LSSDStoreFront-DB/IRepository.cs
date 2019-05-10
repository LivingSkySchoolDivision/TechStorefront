using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.DB
{
    interface IRepository<t>
    {
        t Get(int id);
        List<t> GetAll();
        void Update(t obj);
        void Delete(t obj);
        void UnDelete(t obj);
        void Create(t obj);
        void Create(List<t> obj);
    }
}