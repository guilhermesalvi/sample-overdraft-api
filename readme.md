# 🏦 API de Limite de Conta

[![CI - Build and Test](https://github.com/guilhermesalvi/sample-overdraft-api/actions/workflows/dotnet.yml/badge.svg)](https://github.com/guilhermesalvi/sample-overdraft-api/actions/workflows/dotnet.yml)

### 📚 Sumário

- [📋 Regras de Negócio](#-regras-de-negócio)
- [📆 Cronograma Mensal](#-cronograma-mensal)
- [📊 Regras Práticas de Cobrança](#-regras-práticas-de-cobrança)
- [📘 Glossário](#-glossário)

> 🚧 Este projeto está em desenvolvimento e pode sofrer alterações.

Esta aplicação é responsável por calcular as cobranças mensais de limite de conta (ou cheque especial) com base no uso
diário do cliente.

Diariamente são registrados os valores utilizados do limite de conta e as tarifas correspondentes.

No início do mês subsequente, o sistema calcula as cobranças conforme as regras de negócio e as tarifas aplicadas
anteriormente.

> ⚠️ **Observação:** As regras de negócio aplicadas neste serviço seguem as regulamentações e práticas bancárias
> brasileiras.

## 📆 Cronograma Mensal

```text
[Mês N] (Registro Diário)
└─ O sistema registra diariamente:
    ├─ Utilização do limite de conta
    └─ Cálculo de encargos provisórios:
        ├─ IOF fixo aplicado sobre o maior saldo devedor diário
        ├─ IOF diário proporcional ao saldo devedor
        ├─ Juros remuneratórios sobre o saldo devedor após ultrapassar a carência
        ├─ Juros por excesso de limite (Adiantamento a Depositante - AD)
        ├─ Multa de mora (caso haja saldo devedor ao fim do mês)
        └─ Juros de mora (incidência diária sobre saldo devedor não quitado)

[Mês N+1] (Faturamento)
├─ Consolidação de todos os registros do mês anterior (Mês N)
└─ Emissão da cobrança ao cliente com base nos encargos calculados
```

## 📋 Regras de Negócio

Este serviço aplica as seguintes regras financeiras:

- 🛡️ **Período de Carência**  
  Quantidade de dias em que o cliente pode utilizar o limite de conta sem incidência de juros remuneratórios. Caso
  ultrapassado, os juros passam a ser aplicados retroativamente sobre todos os dias de utilização.

- 📈 **Juros Remuneratórios**  
  Juros diários cobrados sobre o saldo devedor utilizado dentro do limite aprovado, aplicáveis a partir do término do
  período de carência.

- 💰 **IOF (Imposto sobre Operações Financeiras)**
    - **Taxa fixa:** cobrada uma única vez no mês, sobre o maior saldo devedor diário registrado durante o período de
      apuração.
    - **Taxa diária:** cobrada proporcionalmente ao valor do saldo devedor e ao número de dias de utilização do limite
      de conta.

- 🚨 **Juros por Excesso de Limite (Adiantamento a Depositante - AD)**
    - **Taxa fixa:** cobrada uma única vez no mês, caso o cliente exceda o limite de conta aprovado.
    - **Taxa diária:** cobrada proporcionalmente ao valor utilizado que excede o limite de conta aprovado.

- ⏰ **Juros de Mora**  
  Juros diários incidentes sobre o saldo devedor não quitado até o fechamento do ciclo de cobrança anterior.

- ⚠️ **Multa de Mora**  
  Penalidade cobrada uma única vez no início do novo ciclo, quando o cliente mantém saldo devedor não quitado do período
  anterior.

## 📊 Regras Práticas de Cobrança

| Situação                                                      | IOF | Juros Remuneratórios | Juros Excesso de Limite | Multa de Mora | Juros de Mora |
|---------------------------------------------------------------|:---:|:--------------------:|:-----------------------:|:-------------:|:-------------:|
| Não usou limite                                               |  ❌  |          ❌           |            ❌            |       ❌       |       ❌       |
| Utilizou dentro do período de carência                        |  ✅  |          ❌           |            ❌            |       ❌       |       ❌       |
| Utilizou, excedeu carência                                    |  ✅  |          ✅           |            ❌            |       ❌       |       ❌       |
| Utilizou, excedeu limite                                      |  ✅  |          ✅           |            ✅            |       ❌       |       ❌       |
| Utilizou dentro da carência e não quitou até o fim do período |  ✅  |          ❌           |            ❌            |       ✅       |       ✅       |
| Utilizou, excedeu carência e não quitou até o fim do período  |  ✅  |          ✅           |            ❌            |       ✅       |       ✅       |
| Utilizou, excedeu limite e não quitou até o fim do período    |  ✅  |          ✅           |            ✅            |       ✅       |       ✅       |

> ℹ️ Embora os encargos sejam contabilizados diariamente, a cobrança ao cliente ocorre apenas uma vez por mês.

## 📘 Glossário

| Termo                          | Descrição                                                                                                                                                |
|--------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Principal**                  | Valor utilizado do limite de conta pelo cliente.                                                                                                         |
| **Approved Overdraft Limit**   | Limite máximo aprovado para o cliente utilizar no limite de conta.                                                                                       |
| **Self-declared Limit**        | Limite opcional definido pelo cliente para controle pessoal, inferior ou igual ao limite aprovado.                                                       |
| **Used Days In Current Cycle** | Número de dias em que o cliente utilizou o limite de conta no ciclo atual.                                                                               |
| **Used Over-limit**            | Valor utilizado que excede o limite aprovado de conta corrente.                                                                                          |
| **Customer Assets Held**       | Valor dos ativos em custódia do cliente na instituição financeira, utilizados como critério para elegibilidade de produtos.                              |
| **Grace Period Days**          | Número de dias em que o cliente pode usar o limite sem a incidência de juros remuneratórios.                                                             |
| **Interest Rate**              | Taxa aplicada diariamente sobre o saldo devedor utilizado dentro do limite aprovado, após ultrapassado o período de carência **(juros remuneratórios)**. |
| **Credit Tax Rate**            | Taxa diária utilizada para o cálculo do IOF incidente sobre o saldo devedor **(IOF diário)**.                                                            |
| **Fixed Credit Tax Rate**      | Taxa fixa aplicada uma única vez no mês sobre o maior saldo devedor diário registrado **(IOF fixo)**.                                                    |
| **Over-limit Rate**            | Taxa aplicada sobre o valor que excede o limite de conta aprovado **(taxa diária de Adiantamento a Depositante - AD)**.                                  |
| **Over-limit Fixed Fee**       | Tarifa fixa cobrada uma única vez no mês, caso o cliente exceda o limite de conta aprovado **(taxa fixa de Adiantamento a Depositante - AD)**.           |
| **Late Payment Rate**          | Taxa aplicada diariamente sobre o saldo devedor não quitado do mês anterior **(juros de mora)**.                                                         |
| **Penalty Rate**               | Multa única aplicada quando o saldo devedor não é quitado no fechamento do ciclo e é rolado para o próximo mês **(multa de mora)**.                      |
| **Capitalization**             | Processo de incorporação dos encargos ao saldo devedor, formando um novo principal para o ciclo seguinte (gerando juros compostos).                      |
| **Contract**                   | Instrumento contratual que define as condições de uso do limite de conta (valores, taxas, carência, penalidades).                                        |
| **Account**                    | Representação da conta bancária do cliente habilitada para utilizar limite de conta.                                                                     |
| **Contract Agreement**         | Associação formal entre uma conta e um contrato específico, definindo a vigência e regras aplicáveis.                                                    |
| **Daily Limit Usage Entry**    | Registro diário do saldo devedor do cliente, limites aplicáveis e encargos provisórios apurados.                                                         |
| **Monthly Charge Snapshot**    | Registro consolidado dos encargos totais do cliente em um mês, usado para geração da cobrança mensal.                                                    |
| **Product Condition**          | Conjunto de condições que definem a elegibilidade do cliente para produtos de crédito, geralmente baseadas em volume de ativos em custódia.              |


