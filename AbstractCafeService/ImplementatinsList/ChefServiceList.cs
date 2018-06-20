using AbstractCafeModel;
using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;

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
            List<ChefViewModel> result = new List<ChefViewModel>();
            for (int i = 0; i < source.Chefs.Count; ++i)
            {
                result.Add(new ChefViewModel
                {
                    Id = source.Chefs[i].Id,
                    ChefFIO = source.Chefs[i].ChefFIO
                });
            }
            return result;
        }

        public ChefViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Chefs.Count; ++i)
            {
                if (source.Chefs[i].Id == id)
                {
                    return new ChefViewModel
                    {
                        Id = source.Chefs[i].Id,
                        ChefFIO = source.Chefs[i].ChefFIO
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ChefBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Chefs.Count; ++i)
            {
                if (source.Chefs[i].Id > maxId)
                {
                    maxId = source.Chefs[i].Id;
                }
                if (source.Chefs[i].ChefFIO == model.ChefFIO)
                {
                    throw new Exception("Уже есть сотрудник с таким ФИО");
                }
            }
            source.Chefs.Add(new Chef
            {
                Id = maxId + 1,
                ChefFIO = model.ChefFIO
            });
        }

        public void UpdElement(ChefBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Chefs.Count; ++i)
            {
                if (source.Chefs[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Chefs[i].ChefFIO == model.ChefFIO &&
                    source.Chefs[i].Id != model.Id)
                {
                    throw new Exception("Уже есть сотрудник с таким ФИО");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Chefs[index].ChefFIO = model.ChefFIO;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Chefs.Count; ++i)
            {
                if (source.Chefs[i].Id == id)
                {
                    source.Chefs.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
