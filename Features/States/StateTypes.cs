using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.States
{
    public enum StateTypes { Init = 1, ShoppingBagState, ProductTypeSelector, CharacteristicsChanger, BottomTypeSelector, ColorSelector, SizeSelector, SpecificsSelection, ConstructorEnd,
                     DeliveryAddress, PaymentState, RecieverInfo, AdminInitState, AdminControlsState, UserNotificationState, CatalogState}
}
