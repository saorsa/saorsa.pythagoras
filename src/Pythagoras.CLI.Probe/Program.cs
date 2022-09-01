// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Saorsa.Pythagoras.Domain;
using Saorsa.Pythagoras.Domain.Business;
using Saorsa.Pythagoras.Domain.Business.Concrete;
using Saorsa.Pythagoras.Domain.Model;

var svcCollection = new ServiceCollection();

svcCollection.AddPythagoras();

var sp = svcCollection.BuildServiceProvider();
var scope = sp.CreateScope();
var id = scope.ServiceProvider.GetRequiredService<IIdentityProvider>();
((SimpleIdentityProvider)id).SetLoggedInUser(new IdentityContext("adragolov", new []
{
    "sudoers",
    "users",
    "admins",
}));
var categories = scope.ServiceProvider.GetRequiredService<IPythagorasCategoriesService>();
categories.GetRootCategoriesAsync().Wait();
Console.WriteLine("Hello, World!");
scope.Dispose();
sp.Dispose();