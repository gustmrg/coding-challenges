# Desafio Back-end PicPay

### Sobre o ambiente da aplicação:

-   Escolha qualquer framework que se sinta **confortável** em trabalhar. Esse teste **não faz** nenhuma preferência, portanto decida por aquele com o qual estará mais seguro em apresentar e conversar com a gente na entrevista ;)

-   Você pode, inclusive, não optar por framework nenhum. Neste caso, recomendamos a implementação do serviço via script para diminuir a sobrecarga de criar um servidor web.

-   Ainda assim, se optar por um framework tente evitar usar muito métodos mágicos ou atalhos já prontos. Sabemos que essas facilidades aumentam a produtividade no dia-a-dia mas aqui queremos ver o **seu** código e a sua forma de resolver problemas.

-   Valorizamos uma boa estrutura de containeres criada por você.

## Objetivo: PicPay Simplificado

Temos 2 tipos de usuários, os comuns e lojistas, ambos têm carteira com dinheiro e realizam transferências entre eles. Vamos nos atentar **somente** ao fluxo de transferência entre dois usuários.

Requisitos:

-   Para ambos tipos de usuário, precisamos do Nome Completo, CPF, e-mail e Senha. CPF/CNPJ e e-mails devem ser únicos no sistema. Sendo assim, seu sistema deve permitir apenas um cadastro com o mesmo CPF ou endereço de e-mail.

-   Usuários podem enviar dinheiro (efetuar transferência) para lojistas e entre usuários.

-   Lojistas **só recebem** transferências, não enviam dinheiro para ninguém.

-   Validar se o usuário tem saldo antes da transferência.

-   Antes de finalizar a transferência, deve-se consultar um serviço autorizador externo, use este mock para simular (https://run.mocky.io/v3/5794d450-d2e2-4412-8131-73d0293ac1cc).

-   A operação de transferência deve ser uma transação (ou seja, revertida em qualquer caso de inconsistência) e o dinheiro deve voltar para a carteira do usuário que envia.

-   No recebimento de pagamento, o usuário ou lojista precisa receber notificação (envio de email, sms) enviada por um serviço de terceiro e eventualmente este serviço pode estar indisponível/instável. Use este mock para simular o envio (https://run.mocky.io/v3/54dc2cf1-3add-45b5-b5a9-6bf7e7f1f4a6).

-   Este serviço deve ser RESTFul.

### Payload

Faça uma **proposta** :heart: de payload, se preferir, temos uma exemplo aqui:

POST /transaction

```json
{
    "value": 100.0,
    "payer": 4,
    "payee": 15
}
```

# Avaliação

Apresente sua solução utilizando o framework que você desejar, justificando a escolha.

## O que será avaliado e valorizamos :heart:

-   Documentação
-   Se for para vaga sênior, foque bastante no **desenho de arquitetura**
-   Código limpo e organizado (nomenclatura, etc)
-   Conhecimento de padrões (PSRs, design patterns, SOLID)
-   Ser consistente e saber argumentar suas escolhas
-   Apresentar soluções que domina
-   Modelagem de Dados
-   Manutenibilidade do Código
-   Tratamento de erros
-   Cuidado com itens de segurança
-   Arquitetura (estruturar o pensamento antes de escrever)
-   Carinho em desacoplar componentes (outras camadas, service, repository)

De acordo com os critérios acima, iremos avaliar seu teste para avançarmos para a entrevista técnica.
Caso não tenha atingido aceitavelmente o que estamos propondo acima, não iremos prosseguir com o processo.

## O que NÃO será avaliado :warning:

-   Fluxo de cadastro de usuários e lojistas
-   Frontend (só avaliaremos a (API Restful)[https://www.devmedia.com.br/rest-tutorial/28912])
-   Autenticação

## O que será um Diferencial

-   Uso de Docker
-   Testes de [integração](https://www.atlassian.com/continuous-delivery/software-testing/types-of-software-testing)
-   Testes [unitários](https://www.atlassian.com/continuous-delivery/software-testing/types-of-software-testing)
-   Uso de Design Patterns
-   Documentação
-   Proposta de melhoria na arquitetura

# Solução do Desafio Back-end PicPay

### 💻  Introdução
A aplicação "PicPay Simplificado" realiza transferências de dinheiro entre usuários comuns e/ou lojistas. Foi implementada como um serviço RESTful, proporcionando uma experiência simplificada e segura para transações financeiras.

### ⭐️ Funcionalidades do projeto
* Usuários podem enviar dinheiro (efetuar transferência) outros usuários
* Autorização de transações através de consultas em API externa
* Operações tratadas como transações, garantindo atomicidade e consistência

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