using AbstractCafeModel;
using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Data.Entity;

namespace AbstractCafeService.ImplementationsBD
{
    public class MainServiceBD : IMainService
    {
        private AbstractDbContext context;

        public MainServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<ChoiceViewModel> GetList()
        {
            List<ChoiceViewModel> result = context.Choices
                .Select(rec => new ChoiceViewModel
                {
                    Id = rec.Id,
                    CustomerId = rec.CustomerId,
                    MenuId = rec.MenuId,
                    ChefId = rec.ChefId,
                    DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                SqlFunctions.DateName("yyyy", rec.DateCreate),
                    DateImplement = rec.DateImplement == null ? "" :
                                        SqlFunctions.DateName("dd", rec.DateImplement.Value) + " " +
                                        SqlFunctions.DateName("mm", rec.DateImplement.Value) + " " +
                                        SqlFunctions.DateName("yyyy", rec.DateImplement.Value),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    CustomerFIO = rec.Customer.CustomerFIO,
                    MenuName = rec.Menu.MenuName,
                    ChefName = rec.Chef.ChefFIO
                })
                .ToList();
            return result;
        }

        public void CreateChoice(ChoiceBindingModel model)
        {
            context.Choices.Add(new Choice
            {
                CustomerId = model.CustomerId,
                MenuId = model.MenuId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = ChoiceStatus.Принят
            });
            context.SaveChanges();
        }

        public void TakeChoiceInWork(ChoiceBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {

                    Choice element = context.Choices.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    var menuDishs = context.MenuDishs
                                                .Include(rec => rec.Dish)
                                                .Where(rec => rec.MenuId == element.MenuId);
                    // списываем
                    foreach (var menuDish in menuDishs)
                    {
                        int countOnKitchens = menuDish.Count * element.Count;
                        var kitchenDishs = context.KitchenDishs
                                                    .Where(rec => rec.DishId == menuDish.DishId);
                        foreach (var kitchenDish in kitchenDishs)
                        {
                            // компонентов на одном слкаде может не хватать
                            if (kitchenDish.Count >= countOnKitchens)
                            {
                                kitchenDish.Count -= countOnKitchens;
                                countOnKitchens = 0;
                                context.SaveChanges();
                                break;
                            }
                            else
                            {
                                countOnKitchens -= kitchenDish.Count;
                                kitchenDish.Count = 0;
                                context.SaveChanges();
                            }
                        }
                        if (countOnKitchens > 0)
                        {
                            throw new Exception("Не достаточно компонента " +
                                menuDish.Dish.DishName + " требуется " +
                                menuDish.Count + ", не хватает " + countOnKitchens);
                        }
                    }
                    element.ChefId = model.ChefId;
                    element.DateImplement = DateTime.Now;
                    element.Status = ChoiceStatus.Готовится;
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void FinishChoice(int id)
        {
            Choice element = context.Choices.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = ChoiceStatus.Готов;
            context.SaveChanges();
        }

        public void PayChoice(int id)
        {
            Choice element = context.Choices.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = ChoiceStatus.Оплачен;
            context.SaveChanges();
        }

        public void PutDishOnKitchen(KitchenDishBindingModel model)
        {
            KitchenDish element = context.KitchenDishs
                                                .FirstOrDefault(rec => rec.KitchenId == model.KitchenId &&
                                                                    rec.DishId == model.DishId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                context.KitchenDishs.Add(new KitchenDish
                {
                    KitchenId = model.KitchenId,
                    DishId = model.DishId,
                    Count = model.Count
                });
            }
            context.SaveChanges();
        }
    }
}
