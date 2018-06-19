using AbstractCafeModel;
using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractCafeService.ImplementationsBD
{
    public class KitchenServiceBD : IKitchenService
    {
        private AbstractDbContext context;

        public KitchenServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<KitchenViewModel> GetList()
        {
            List<KitchenViewModel> result = context.Kitchens
                .Select(rec => new KitchenViewModel
                {
                    Id = rec.Id,
                    KitchenName = rec.KitchenName,
                    KitchenDishs = context.KitchenDishs
                            .Where(recPC => recPC.KitchenId == rec.Id)
                            .Select(recPC => new KitchenDishViewModel
                            {
                                Id = recPC.Id,
                                KitchenId = recPC.KitchenId,
                                DishId = recPC.DishId,
                                DishName = recPC.Dish.DishName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public KitchenViewModel GetElement(int id)
        {
            Kitchen element = context.Kitchens.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new KitchenViewModel
                {
                    Id = element.Id,
                    KitchenName = element.KitchenName,
                    KitchenDishs = context.KitchenDishs
                            .Where(recPC => recPC.KitchenId == element.Id)
                            .Select(recPC => new KitchenDishViewModel
                            {
                                Id = recPC.Id,
                                KitchenId = recPC.KitchenId,
                                DishId = recPC.DishId,
                                DishName = recPC.Dish.DishName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(KitchenBindingModel model)
        {
            Kitchen element = context.Kitchens.FirstOrDefault(rec => rec.KitchenName == model.KitchenName);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            context.Kitchens.Add(new Kitchen
            {
                KitchenName = model.KitchenName
            });
            context.SaveChanges();
        }

        public void UpdElement(KitchenBindingModel model)
        {
            Kitchen element = context.Kitchens.FirstOrDefault(rec =>
                                        rec.KitchenName == model.KitchenName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть кухня с таким названием");
            }
            element = context.Kitchens.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.KitchenName = model.KitchenName;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Kitchen element = context.Kitchens.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        // при удалении удаляем все записи о компонентах на удаляемом складе
                        context.KitchenDishs.RemoveRange(
                                            context.KitchenDishs.Where(rec => rec.KitchenId == id));
                        context.Kitchens.Remove(element);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Элемент не найден");
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
