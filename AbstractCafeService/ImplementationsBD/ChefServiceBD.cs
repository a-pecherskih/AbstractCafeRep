using AbstractCafeModel;
using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractCafeService.ImplementationsBD
{
    public class ChefServiceBD : IChefService
    {
        private AbstractDbContext context;

        public ChefServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<ChefViewModel> GetList()
        {
            List<ChefViewModel> result = context.Chefs
                .Select(rec => new ChefViewModel
                {
                    Id = rec.Id,
                    ChefFIO = rec.ChefFIO
                })
                .ToList();
            return result;
        }

        public ChefViewModel GetElement(int id)
        {
            Chef element = context.Chefs.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new ChefViewModel
                {
                    Id = element.Id,
                    ChefFIO = element.ChefFIO
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ChefBindingModel model)
        {
            Chef element = context.Chefs.FirstOrDefault(rec => rec.ChefFIO == model.ChefFIO);
            if (element != null)
            {
                throw new Exception("Уже есть шеф с таким ФИО");
            }
            context.Chefs.Add(new Chef
            {
                ChefFIO = model.ChefFIO
            });
            context.SaveChanges();
        }

        public void UpdElement(ChefBindingModel model)
        {
            Chef element = context.Chefs.FirstOrDefault(rec =>
                                        rec.ChefFIO == model.ChefFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть шеф с таким ФИО");
            }
            element = context.Chefs.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.ChefFIO = model.ChefFIO;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Chef element = context.Chefs.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Chefs.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
