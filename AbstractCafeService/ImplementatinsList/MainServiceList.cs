using AbstractCafeModel;
using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractCafeService.ImplementatinsList
{
    public class MainServiceList : IMainService
    {
        private DataListSingleton source;

        public MainServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ChoiceViewModel> GetList()
        {
            List<ChoiceViewModel> result = source.Choices
                .Select(rec => new ChoiceViewModel
                {
                    Id = rec.Id,
                    CustomerId = rec.CustomerId,
                    MenuId = rec.MenuId,
                    ChefId = rec.ChefId,
                    DateCreate = rec.DateCreate.ToLongDateString(),
                    DateImplement = rec.DateImplement?.ToLongDateString(),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    CustomerFIO = source.Customers
                                    .FirstOrDefault(recC => recC.Id == rec.CustomerId)?.CustomerFIO,
                    MenuName = source.Menus
                                    .FirstOrDefault(recP => recP.Id == rec.MenuId)?.MenuName,
                    ChefName = source.Chefs
                                    .FirstOrDefault(recI => recI.Id == rec.ChefId)?.ChefFIO
                })
                .ToList();
            return result;
        }

        public void CreateChoice(ChoiceBindingModel model)
        {
            int maxId = source.Choices.Count > 0 ? source.Choices.Max(rec => rec.Id) : 0;
            source.Choices.Add(new Choice
            {
                Id = maxId + 1,
                CustomerId = model.CustomerId,
                MenuId = model.MenuId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = ChoiceStatus.Принят
            });
        }

        public void TakeChoiceInWork(ChoiceBindingModel model)
        {
            Choice element = source.Choices.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }

            var menuDishs = source.MenuDishs.Where(rec => rec.MenuId == element.MenuId);
            foreach (var menuDish in menuDishs)
            {
                int countOnKitchens = source.KitchenDishs
                                            .Where(rec => rec.DishId == menuDish.DishId)
                                            .Sum(rec => rec.Count);
                if (countOnKitchens < menuDish.Count * element.Count)
                {
                    var dishName = source.Dishs
                                    .FirstOrDefault(rec => rec.Id == menuDish.DishId);
                    throw new Exception("Недостаточно ингредиентов " + dishName?.DishName +
                        " требуется " + menuDish.Count + ", в наличии " + countOnKitchens);
                }
            }

            foreach (var menuDish in menuDishs)
            {
                int countOnKitchens = menuDish.Count * element.Count;
                var kitchenDishs = source.KitchenDishs
                                            .Where(rec => rec.DishId == menuDish.DishId);
                foreach (var kitchenDish in kitchenDishs)
                {

                    if (kitchenDish.Count >= countOnKitchens)
                    {
                        kitchenDish.Count -= countOnKitchens;
                        break;
                    }
                    else
                    {
                        countOnKitchens -= kitchenDish.Count;
                        kitchenDish.Count = 0;
                    }
                }
            }
            element.ChefId = model.ChefId;
            element.DateImplement = DateTime.Now;
            element.Status = ChoiceStatus.Готовится;
        }

        public void FinishChoice(int id)
        {
            Choice element = source.Choices.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = ChoiceStatus.Готов;
        }

        public void PayChoice(int id)
        {
            Choice element = source.Choices.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = ChoiceStatus.Оплачен;
        }

        public void PutDishOnKitchen(KitchenDishBindingModel model)
        {
            KitchenDish element = source.KitchenDishs
                                                .FirstOrDefault(rec => rec.KitchenId == model.KitchenId &&
                                                                    rec.DishId == model.DishId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                int maxId = source.KitchenDishs.Count > 0 ? source.KitchenDishs.Max(rec => rec.Id) : 0;
                source.KitchenDishs.Add(new KitchenDish
                {
                    Id = ++maxId,
                    KitchenId = model.KitchenId,
                    DishId = model.DishId,
                    Count = model.Count
                });
            }
        }
    }
}
