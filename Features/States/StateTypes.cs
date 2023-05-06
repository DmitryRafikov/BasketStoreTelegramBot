namespace BasketStoreTelegramBot.States
{
    public enum StateTypes
    {
        Init = 1,
        ShoppingBagState,
        ProductTypeSelector, 
        CharacteristicsChanger, 
        BottomTypeSelector, 
        ColorSelector, 
        ChooseSizeType, 
        SpecificsSelection, 
        ConstructorEnd,
        DeliveryAddress, 
        PaymentState, 
        RecieverInfo, 
        AdminInitState, 
        AdminControlsState, 
        UserNotificationState, 
        CatalogState,
        ConstantOwnSizesCreator,
        CustomOwnSizesCreator,
        ExistingSizeSelector
    }
}
