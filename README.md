# test-examples-dotnet

Exemplos da aplicação de testes unitários e de integração com .NET.

# Principais bibliotecas usadas

- [FluentAssertions](https://fluentassertions.com/): facilita a leitura e elaboração dos *asserts*.
  
  - Nuget: https://www.nuget.org/packages/FluentAssertions
- [FakeItEasy](https://fakeiteasy.github.io/): é um *framework* para criação de todos os tipos de *fake objects*, *mocks*, *stubs* etc.
  - Nuget: https://www.nuget.org/packages/FakeItEasy

- [Autofac.Extras.FakeItEasy](https://autofac.readthedocs.io/en/latest/integration/fakeiteasy.html): permite a criação automática de *fake dependencies* para instâncias concretas e abstratas nos testes unitários usando um contêiner Autofac. A classe `AutoFake` injeta as *fake dependencies* para o funcionamento de uma classe. Isso facilita muito, pois ao invés de criar cada *fake dependency* individualmente e passar para no construtor de uma classe, a classe `AutoFake` faz isso de maneira automática.
  
  - Nuget: https://www.nuget.org/packages/Autofac.Extras.FakeItEasy

- [RichardSzalay.MockHttp](https://github.com/richardszalay/mockhttp): fornece um conjunto de recursos de testes que tornam os testes com HttpClient mais fáceis. É possível fazer validações e forçar retornos nas chamadas HTTP, através de uma classe chamada `MockHttpMessageHandler`. 
  
  - Nuget: https://www.nuget.org/packages/RichardSzalay.MockHttp

- 

# Referências

- Teoria:
  - [Test Doubles (Mocks, Stubs, Fakes, Spies e Dummies)](https://medium.com/rd-shipit/test-doubles-mocks-stubs-fakes-spies-e-dummies-a5cdafcd0daf)
  - [Microsoft - Unit testing best practices with .NET Core and .NET Standard](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
- Validações de Log com FakeItEasy:
  - [c# - Test ILogger with FakeItEasy - Stack Overflow](https://stackoverflow.com/questions/64404028/test-ilogger-with-fakeiteasy)
  
  - [A.CallTo(() =&gt; ...).MustHaveHappened(2, Times.Exactly) fails for unknown reason after upgrading to netcoreapp3.0 · Issue #1650 · FakeItEasy/FakeItEasy · GitHub](https://github.com/FakeItEasy/FakeItEasy/issues/1650)