using AbstractCafeModel;
using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;

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
            List<ChoiceViewModel> result = new List<ChoiceViewModel>();
            for (int i = 0; i < source.Choices.Count; ++i)
            {
                string customerFIO = string.Empty;
                for (int j = 0; j < source.Customers.Count; ++j)
                {
                    if (source.Customers[j].Id == source.Choices[i].CustomerId)
                    {
                        customerFIO = source.Customers[j].CustomerFIO;
                        break;
                    }
                }
                string menuName = string.Empty;
                for (int j = 0; j < source.Menus.Count; ++j)
                {
                    if (source.Menus[j].Id == source.Menus[i].Id)
                    {
                        menuName = source.Menus[j].MenuName;
                        break;
                    }
                }
                string chefFIO = string.Empty;
                if (source.Choices[i].ChefId.HasValue)
                {
                    for (int j = 0; j < source.Chefs.Count; ++j)
                    {
                        if (source.Chefs[j].Id == source.Choices[i].ChefId.Value)
                        {
                            chefFIO = source.Chefs[j].ChefFIO;
                            break;
                        }
                    }
                }
                result.Add(new ChoiceViewModel
                {
                    Id = source.Choices[i].Id,
                    CustomerId = source.Choices[i].CustomerId,
                    CustomerFIO = customerFIO,
                    MenuId = source.Choices[i].MenuId,
                    MenuName = menuName,
                    ChefId = source.Choices[i].ChefId,
                    ChefName = chefFIO,
                    Count = source.Choices[i].Count,
                    Sum = source.Choices[i].Sum,
                    DateCreate = source.Choices[i].DateCreate.ToLongDateString(),
                    DateImplement = source.Choices[i].DateImplement?.ToLongDateString(),
                    Status = source.Choices[i].Status.ToString()
                });
            }
            return result;
        }

        public void CreateChoice(ChoiceBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Choices.Count; ++i)
            {
                if (source.Choices[i].Id > maxId)
                {
                    maxId = source.Customers[i].Id;
                }
            }
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
            int index = -1;
            for (int i = 0; i < source.Choices.Count; ++i)
            {
                if (source.Choices[i].Id == model.Id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            // смотрим по количеству компонентов на складах
            for (int i = 0; i < source.MenuDishs.Count; ++i)
            {
                if (source.MenuDishs[i].MenuId == source.Choices[index].MenuId)
                {
                    int countOnKitchens = 0;
                    for (int j = 0; j < source.KitchenDishs.Count; ++j)
                    {
                        if (source.KitchenDishs[j].DishId == source.MenuDishs[i].DishId)
                        {
                            countOnKitchens += source.KitchenDishs[j].Count;
                        }
                    }
                    if (countOnKitchens < source.MenuDishs[i].Count)
                    {
                        for (int j = 0; j < source.Dishs.Count; ++j)
                        {
                            if (source.Dishs[j].Id == source.MenuDishs[i].DishId)
                            {
                                throw new Exception("Не достаточно компонента " + source.Dishs[j].DishName +
                                    " требуется " + source.MenuDishs[i].Count + ", в наличии " + countOnKitchens);
                            }
                        }
                    }
                }
            }
            // списываем
            for (int i = 0; i < source.MenuDishs.Count; ++i)
            {
                if (source.MenuDishs[i].MenuId == source.Choices[index].MenuId)
                {
                    int countOnKitchens = source.MenuDishs[i].Count;
                    for (int j = 0; j < source.KitchenDishs.Count; ++j)
                    {
                        if (source.KitchenDishs[j].DishId == source.KitchenDishs[i].DishId)
                        {
                            // компонентов на одном слкаде может не хватать
                            if (source.KitchenDishs[j].Count >= countOnKitchens)
                            {
                                source.KitchenDishs[j].Count -= countOnKitchens;
                                break;
                            }
                            else
                            {
                                countOnKitchens -= source.KitchenDishs[j].Count;
                                source.KitchenDishs[j].Count = 0;
                            }
                        }
                    }
                }
            }
            source.Choices[index].ChefId = model.ChefId;
            source.Choices[index].DateImplement = DateTime.Now;
            source.Choices[index].Status = ChoiceStatus.Готовится;
        }

        public void FinishChoice(int id)
        {
            int index = -1;
            for (int i = 0; i < source.Choices.Count; ++i)
            {
                if (source.Customers[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Choices[index].Status = ChoiceStatus.Готов;
        }

        public void PayChoice(int id)
        {
            int index = -1;
            for (int i = 0; i < source.Choices.Count; ++i)
            {
                if (source.Customers[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Choices[index].Status = ChoiceStatus.Оплачен;
        }

        public void PutDishOnKitchen(KitchenDishBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.KitchenDishs.Count; ++i)
            {
                if (source.KitchenDishs[i].KitchenId == model.KitchenId &&
                    source.KitchenDishs[i].DishId == model.DishId)
                {
                    source.KitchenDishs[i].Count += model.Count;
                    return;
                }
                if (source.KitchenDishs[i].Id > maxId)
                {
                    maxId = source.KitchenDishs[i].Id;
                }
            }
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
