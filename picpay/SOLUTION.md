# Solução do Desafio PicPay

### 💻  Introdução
A aplicação "PicPay Simplificado" realiza transferências de dinheiro entre usuários comuns e/ou lojistas. Foi implementada como um serviço RESTful, proporcionando uma experiência simplificada e segura para transações financeiras.

### ⭐️ Funcionalidades do projeto
* Usuários podem enviar dinheiro (efetuar transferência) outros usuários
* Autorização de transações através de consultas em API externa
* Operações tratadas como transações, garantindo atomicidade e consistência

### ⚙️ Instalação
* Clone this repository [here](https://github.com/).


### 🚀 Execução
...

### ⛓ API Endpoints
| HTTP Verbs | Endpoints | Action |
| --- | --- | --- |
| GET | /transactions/:transactionId | Recupera os detalhes de uma transação financeira |
| POST | /transactions | Cria uma nova transação financeira entre carteiras |
| GET | /users | Retorna a lista de usuários |
| POST | /users | Cria um novo usuário |
| GET | /users/:userId | Retorna os detalhes de um usuário |

### ✨ Tecnologias
* [.NET](https://dotnet.microsoft.com/): O .NET é uma plataforma gratuita para desenvolvedores, multiplataforma e de software livre, que permite criar vários tipos de aplicativos. O .NET é criado em um runtime de alto desempenho que é usado em produção por muitos aplicativos de alta escala.
* [Entity Framework Core](https://learn.microsoft.com/pt-br/ef/core/): O EF (Entity Framework) Core é uma versão leve, extensível, de software livre e multiplataforma da popular tecnologia de acesso a dados do Entity Framework.
* [Sqlite](https://www.sqlite.org/): SQLite é uma biblioteca de linguagem C que implementa um motor de banco de dados SQL pequeno, rápido, independente, de alta confiabilidade e completo.

### 📄 Licença
Esse projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE.md) para mais detalhes.