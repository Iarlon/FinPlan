﻿# Projeto Financeiro

Este é um projeto de gestão financeira desenvolvido em C#/.NET. A aplicação foi estruturada focando em boas práticas de engenharia de software, utilizando princípios de separação de responsabilidades, como **Domain-Driven Design (DDD)** e **Clean Architecture**.
Está disponível no link = http://planfin.tech

<img src="http://localhost:9000/api/project_badges/measure?project=projetoLocal&metric=alert_status&token=sqb_7f12579150574b1eadbd6f3494d0c494d015f702">

## 🏗️ Arquitetura e Padrões de Projeto

O projeto foi construído seguindo os princípios da **Clean Architecture** e **Domain-Driven Design (DDD)**, utilizando os seguintes padrões:

*   **DDD (Domain-Driven Design):** Divisão clara em camadas de Domain, Application e Infrastructure. Entidades ricas com validações internas (ex: `Meta`, `Movimentacao`).
*   **CQRS (Command Query Responsibility Segregation):** Separação das operações de leitura e escrita para escalabilidade e clareza.
*   **MediatR:** Utilizado para desacoplamento de componentes através do padrão Mediator, facilitando a implementação de comandos e consultas.
*   **Domain Events:** Notificação de mudanças de estado dentro do domínio (ex: `MovimentacaoCriadaEvent`), permitindo efeitos colaterais de forma desacoplada.
*   **Repository Pattern:** Abstração da camada de dados para facilitar a testabilidade e troca de provedores de persistência.
*   **Dependency Injection:** Uso extensivo de DI nativa do .NET para gestão de ciclo de vida de objetos.

### 🔒 Segurança e Infraestrutura

*   **CORS (Cross-Origin Resource Sharing):** Configurado para permitir o consumo seguro da API por diferentes origens (front-end).
*   **Containerização:** Uso de **Docker** e **Docker Compose** para orquestração de serviços e padronização do ambiente de desenvolvimento/produção.

## 🚀 Tecnologias Utilizadas

*   **.NET 8 (C#):** Plataforma de desenvolvimento.
*   **PostgreSQL:** Banco de dados relacional.
*   **Docker & Docker Compose:** Containerização da aplicação e do banco de dados.
*   **Swagger/OpenAPI:** Documentação interativa da API com suporte a JWT.
*   **xUnit & Moq:** Frameworks para testes unitários e mocking.
*   **FluentValidation:** Validação de inputs de forma elegante.

## 🏗️ Estrutura do Projeto

A solução está dividida principalmente nas seguintes camadas:

* **`financeiro` (Projeto Principal)**
  * **Domain:** Contém as entidades principais do negócio, regras estritas do domínio e validações (ex: `Usuario`).
  * **Application:** Contém as regras de negócio da aplicação e orquestração de chamadas (ex: `CriarUsuario`, serviços e casos de uso).
  * **Infrastructure / API:** Camadas de acesso a banco de dados, injeção de dependência e exposição dos endpoints.

* **`financeiro.Tests` (Projeto de Testes)**
  * **Tests/Domain:** Testes unitários para garantir que as regras das entidades de domínio estão corretas (ex: `UsuarioTest`).
  * **Tests/Application:** Testes unitários e de integração validando os serviços da aplicação, como a criação de usuários (ex: `UsuarioServiceTest`).

## ⚙️ Pré-requisitos

Para executar e contribuir com este projeto, certifique-se de ter os seguintes itens instalados no seu ambiente:

* [.NET SDK](https://dotnet.microsoft.com/download) (Recomendado a versão mais recente suportada pelo projeto, ex: .NET 6, 7 ou 8)
* [Visual Studio](https://visualstudio.microsoft.com/) ou [Visual Studio Code](https://code.visualstudio.com/)

## 🏃 Como Executar o Projeto

1. Clone o repositório para sua máquina local.
2. Navegue até a pasta raiz do projeto via terminal.
3. Restaure as dependências do pacote executando:
 ```dotnet restore```
4. Para compilar o projeto, execute:
 ```dotnet build```
5. Para rodar a bateria de testes unitários é preciso primeiramente ir à pasta dos testes e após isso rodar os testes:
 ```
	cd financeiro.Tests
	dotnet test
 ```
 -- EXTRA: se rodar esse código você consegue ter mais detalhes dos testes, sendo por exemplo com extensão que pode ser usada no Visual Studio como trx:
 ```
 dotnet test --logger "trx;LogFileName=TestResults.trx"
 ```
 Ficando assim: <br>
 <img src="./financeiro.Tests/assets/usingTestResult.png" width="600">

6. Para executar a aplicação principal:
 ```dotnet run --project financeiro```
 Ou ir ao diretório do projeto `financeiro` e executar:
 ```dotnet run```


## Evidências de testes:
<img src="./financeiro.Tests/assets/readmeImage.png" width="600">
