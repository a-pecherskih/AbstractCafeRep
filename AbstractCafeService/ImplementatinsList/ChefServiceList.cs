using AbstractCafeModel;
using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractCafeService.ImplementatinsList
{
    public class ChefServiceList : IChefService
    {
        private DataListSingleton source;

        public ChefServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ChefViewModel> GetList()
        {
            List<ChefViewModel> result = source.Chefs
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
            Chef element = source.Chefs.FirstOrDefault(rec => rec.Id == id);
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
            Chef element = source.Chefs.FirstOrDefault(rec => rec.ChefFIO == model.ChefFIO);
            if (element != null)
            {
                throw new Exception("Уже есть шеф с таким ФИО");
            }
            int maxId = source.Chefs.Count > 0 ? source.Chefs.Max(rec => rec.Id) : 0;
            source.Chefs.Add(new Chef
            {
                Id = maxId + 1,
                ChefFIO = model.ChefFIO
            });
        }

        public void UpdElement(ChefBindingModel model)
        {
            Chef element = source.Chefs.FirstOrDefault(rec =>
                                        rec.ChefFIO == model.ChefFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть шеф с таким ФИО");
            }
            element = source.Chefs.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.ChefFIO = model.ChefFIO;
        }

        public void DelElement(int id)
        {
            Chef element = source.Chefs.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Chefs.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
