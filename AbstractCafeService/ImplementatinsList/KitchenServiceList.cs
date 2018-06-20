using AbstractCafeModel;
using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractCafeService.ImplementatinsList
{
    public class KitchenServiceList : IKitchenService
    {
        private DataListSingleton source;

        public KitchenServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<KitchenViewModel> GetList()
        {
            List<KitchenViewModel> result = source.Kitchens
                .Select(rec => new KitchenViewModel
                {
                    Id = rec.Id,
                    KitchenName = rec.KitchenName,
                    KitchenDishs = source.KitchenDishs
                            .Where(recPC => recPC.KitchenId == rec.Id)
                            .Select(recPC => new KitchenDishViewModel
                            {
                                Id = recPC.Id,
                                KitchenId = recPC.KitchenId,
                                DishId = recPC.DishId,
                                DishName = source.Dishs
                                    .FirstOrDefault(recC => recC.Id == recPC.DishId)?.DishName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public KitchenViewModel GetElement(int id)
        {
            Kitchen element = source.Kitchens.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new KitchenViewModel
                {
                    Id = element.Id,
                    KitchenName = element.KitchenName,
                    KitchenDishs = source.KitchenDishs
                            .Where(recPC => recPC.KitchenId == element.Id)
                            .Select(recPC => new KitchenDishViewModel
                            {
                                Id = recPC.Id,
                                KitchenId = recPC.KitchenId,
                                DishId = recPC.DishId,
                                DishName = source.Dishs
                                    .FirstOrDefault(recC => recC.Id == recPC.DishId)?.DishName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(KitchenBindingModel model)
        {
            Kitchen element = source.Kitchens.FirstOrDefault(rec => rec.KitchenName == model.KitchenName);
            if (element != null)
            {
                throw new Exception("Уже есть кухня с таким названием");
            }
            int maxId = source.Kitchens.Count > 0 ? source.Kitchens.Max(rec => rec.Id) : 0;
            source.Kitchens.Add(new Kitchen
            {
                Id = maxId + 1,
                KitchenName = model.KitchenName
            });
        }

        public void UpdElement(KitchenBindingModel model)
        {
            Kitchen element = source.Kitchens.FirstOrDefault(rec =>
                                        rec.KitchenName == model.KitchenName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть кухня с таким названием");
            }
            element = source.Kitchens.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.KitchenName = model.KitchenName;
        }

        public void DelElement(int id)
        {
            Kitchen element = source.Kitchens.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.KitchenDishs.RemoveAll(rec => rec.KitchenId == id);
                source.Kitchens.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
