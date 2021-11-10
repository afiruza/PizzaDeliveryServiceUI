sl C:\Projects\PizzaDeliveryServiceUI\
& dotnet restore
& dotnet publish --configuration Release
& Import-Module WebAdministration; Set-ItemProperty 'IIS:\sites\Default Web Site' -Name physicalPath -Value C:\Projects\PizzaDeliveryServiceUI\bin\release\netcoreapp3.1\publish