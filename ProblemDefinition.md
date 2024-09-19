# C# DO JEITO CERTO DESAFIO FINAL

## CONTEXTO GERAL DO PROBLEMA

### Colocar em prática principais conceitos
Este exercício tem como objetivo colocar em prática, de forma conectada, os assuntos abordados durante o treinamento.

### Exemplo mais próximo do mundo real
Buscamos uma cenário mais próximo possível do mundo real para tangibilizar o impacto das práticas apresentadas durante o treinamento.

### Controle de pedidos para um e-commerce
Você foi contratado para desenvolver uma aplicação para a gestão de pedidos em uma plataforma de e-commerce. A plataforma deve permitir que os usuários realizem pedidos de produtos, acompanhem o status desses pedidos e interajam com diferentes serviços dentro do sistema, como estoque, pagamento e entrega.

## REGRAS DE FLUXO DE PEDIDOS

### 1) Recepção de pedidos
- O usuário pode criar um pedido com um ou mais itens. Cada item tem um preço e quantidade.
- O sistema deve calcular o valor total do pedido e aplicar possíveis descontos com base em
regras de negócio de cada produto. As regras existentes são:
    - Desconto por quantidade : Desconto em Reais para quantidades específicas;
    - Desconto Sazonal: Conforme data do pedido, desconto em percentual do total
- Um pedido criado com sucesso, fica em estado "Aguardando processamento".
- Pedidos que estão aguardando processamento pode ser cancelados, consequentemente irão para o estado "Cancelado".

### 2) Processando pagamento
- Pedidos que estão em "Aguardando processamento", devem entrar em novo estado que é "Processando Pagamento".
- Pedidos em estado: "Processando Pagamento" devem acionar uma estratégia de pagamento conforme dados do pedido, são elas:
    - Pagamento à vista com Pix com desconto 5%
    - Pagamento parcelado em até 12x no cartão
- Pagamento realizado com sucesso move pedido para estado "Pagamento Concluído"
- Pedidos em estado posterior ao de "Pagamento Concluído", podem ser cancelados, desde que exista uma operação de Estorno.
- Em caso de falha no processamento de pagamento realiza 3 tentativas em caso de falha de comunicação. Ao falhar pedido vai para estado cancelado.

### 3) Separando Pedido
- Pedidos que estão em "Pagamento Concluído", devem entrar em novo estado que é "Separando Pedido".
- Pedidos em estado: "Separando Pedido" devem fazer a baixa dos produtos em estoque. Caso produto, não possua estoque enviar um email para vendas alertando a situação.
- Pedidos com produtos separados, com sucesso, vão para o estado de "Concluído".
- Pedidos que tiveram problema, na separação de itens, devem ir para o estado "Aguardando Estoque".
- Pedidos que estão como "Concluído", não podem ser cancelados.
- Pedidos que estão como "Aguardando Estoque" podem ser cancelados, realizando o Estorno de pagamento corretamente.

## REGRAS DE NOTIFICAÇÕES

- Cada troca de estado deve notificar o cliente, via email, o que aconteceu com o pedido.
- 1 vez por dia gerar uma lista de pedidos do dia anterior e enviar para um endereço de email do dono do produto.

## RESTRIÇÕES TÉCNICAS

- Testes de unidade nos serviços de domínio e entidades
- Testes de integração nas operações de controle de pedidos
- Decisão de design, modelagem e tecnologias ficam de responsabilidade de cada desenvolvedor
- Subir o código para um repositório do Github