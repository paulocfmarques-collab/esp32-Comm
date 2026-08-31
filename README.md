# CommEspe32

Aplicativo Windows desenvolvido em **C# WinForms** para comunicação UDP com dispositivos **ESP32**.

O objetivo do projeto é fornecer uma ferramenta simples para envio de comandos e monitoramento de respostas em tempo real através da rede, facilitando testes, desenvolvimento, depuração e manutenção de aplicações embarcadas.

![Platform](https://shields.io)
![Language](https://img.shields.io/badge/Language-C%23-green)
![.NET](https://img.shields.io/badge/.NET-WinForms-purple)
![Protocol](https://img.shields.io/badge/Protocol-UDP-orange)
![License](https://shields.io)


---

# Visão Geral

O CommEspe32 permite que um computador envie comandos UDP para um ESP32 conectado à rede local e exiba automaticamente as respostas retornadas pelo dispositivo.

O programa abre um socket UDP local, mantém uma thread de recepção ativa e exibe todas as respostas recebidas em uma janela de log com rolagem automática.

---

# Recursos

✅ Configuração de IP e porta de destino

✅ Validação de endereço IPv4

✅ Validação de porta numérica

✅ Comunicação UDP bidirecional

✅ Envio de comandos personalizados

✅ Recepção assíncrona de mensagens

✅ Atualização automática do log

✅ Auto-scroll da janela de respostas

✅ Interface simples e leve

✅ Compatível com ESP32 Ethernet e Wi-Fi

---

# Interface

A aplicação possui os seguintes componentes:

| Componente | Função |
|------------|---------|
| IP | Endereço do ESP32 |
| Porta | Porta UDP utilizada na comunicação |
| Listen | Inicia ou encerra a comunicação |
| Comando | Texto a ser enviado ao ESP32 |
| Envia | Transmite o comando |
| Resposta | Exibe as mensagens recebidas |

---

# Fluxo de Funcionamento

```text
┌──────────────────────┐
│ Informar IP e Porta  │
└──────────┬───────────┘
           │
           ▼
┌──────────────────────┐
│ Iniciar Listen       │
└──────────┬───────────┘
           │
           ▼
┌──────────────────────┐
│ Socket UDP Aberto    │
└──────────┬───────────┘
           │
           ▼
┌──────────────────────┐
│ Digitar Comando      │
└──────────┬───────────┘
           │
           ▼
┌──────────────────────┐
│ Enviar para ESP32    │
└──────────┬───────────┘
           │
           ▼
┌──────────────────────┐
│ ESP32 Processa       │
└──────────┬───────────┘
           │
           ▼
┌──────────────────────┐
│ ESP32 Responde       │
└──────────┬───────────┘
           │
           ▼
┌──────────────────────┐
│ Log Atualizado       │
└──────────────────────┘
```

---

# Arquitetura

```text
+---------------------------------+
|           CommEspe32            |
|             WinForms            |
+---------------+-----------------+
                |
                |
                | UDP
                |
                v
+---------------------------------+
|             ESP32               |
|       Wi-Fi ou Ethernet         |
+---------------------------------+
```

---

# Requisitos

## Sistema Operacional

- Windows 7
- Windows 8
- Windows 10
- Windows 11

## Desenvolvimento

- Visual Studio 2019 ou superior
- .NET Framework

---

# Compilação

Clone o projeto:

```bash
git clone https://github.com/SEU_USUARIO/CommEspe32.git
```

Abra a solução no Visual Studio e execute:

```text
Build → Rebuild Solution
```

Ou:

```text
Ctrl + Shift + B
```

Para executar:

```text
F5
```

---

# Como Utilizar

## 1. Configurar o ESP32

Configure o firmware para receber comandos UDP.

Exemplo:

```text
IP: 192.168.0.100
Porta: 5000
```

---

## 2. Configurar o Aplicativo

Preencha:

```text
IP: 192.168.0.100
Porta: 5000
```

Clique em:

```text
Listen
```

Quando a comunicação estiver ativa:

- O botão muda para o ícone de parada.
- Os controles de configuração são bloqueados.
- A recepção de mensagens é iniciada.

---

## 3. Enviar Comandos

Digite um comando no campo:

```text
Comando
```

Exemplo:

```text
CPU
```

Clique em:

```text
Envia
```

---

# Exemplo de Comunicação

## Solicitar as informações da CPU

Comando:

```text
CPU
```

Resposta:

```text
Modelo: ESP32-D0WD-V3
Revisao: 301
Nucleos: 2
CPU: 240 MHz
RAM livre: 230136 bytes
```

---

## Solicitar Memória Livre

Comando:

```text
RAM
```

Resposta:

```text
Heap livre: 230136
Menor heap livre: 146544
Maior bloco livre: 110580
```

---

## Solicitar Informações Gerais

Comando:

```text
NET_INFO
```

Resposta:

```text
IP: 192.168.0.120
Gateway: 192.168.0.1
Mascara de rede: 255.255.255.0
RSSI: -28 dbm
Nome da Rede: xxxxxxxxxxxx
```

---

# Exemplo de Firmware ESP32

## Recepção

```cpp
int packetSize = udp.parsePacket();

if(packetSize)
{
    char buffer[256];

    int len = udp.read(buffer, sizeof(buffer) - 1);

    if(len > 0)
        buffer[len] = '\0';

    Serial.println(buffer);
}
```

## Resposta

```cpp
udp.beginPacket(
    udp.remoteIP(),
    udp.remotePort()
);

udp.print("OK");

udp.endPacket();
```

---

# Comandos Sugeridos

Dependendo do firmware implementado:

```text
RESET_WIFI
LED_ON
LED_OFF
LED_PISCA:XXX:YYY
LED_BLINK:YYY
TEMP
CPU
RAM
FLASH
INIT
UPTIME
MAC
NET_INFO
```

---

# Casos de Uso

- Desenvolvimento de firmware ESP32
- Testes de comunicação UDP
- Monitoramento remoto
- Sistemas IoT
- Automação residencial
- Controle de dispositivos embarcados
- Diagnóstico de equipamentos
- Redes industriais
- Laboratórios de ensino

---

# Estrutura do Projeto

```text
CommEspe32
│
├── Program.cs
├── Form1.cs
├── Form1.Designer.cs
│
├── Properties
│   ├── Resources.resx
│   ├── Resources.Designer.cs
│   └── Settings.settings
│
├── README.md
│
└── CommEspe32.sln
```

---

# Funcionalidades Implementadas

- [x] Comunicação UDP
- [x] Thread dedicada para recepção
- [x] Validação de IP
- [x] Validação de porta
- [x] Transmissão de comandos
- [x] Exibição de respostas
- [x] Auto-scroll do log
- [x] Controle de conexão
- [x] Interface gráfica WinForms

---

# Melhorias Futuras

- [ ] Histórico de comandos
- [ ] Salvar configurações automaticamente
- [ ] Exportar logs para arquivo
- [ ] Múltiplos dispositivos simultâneos
- [ ] Descoberta automática de dispositivos na rede
- [ ] Comunicação TCP
- [ ] Comunicação MQTT
- [ ] Comunicação BLE
- [ ] Interface Dark Mode
- [ ] Atualização OTA do ESP32

---

# Licença

Este projeto é distribuído para fins educacionais, de estudo e desenvolvimento de sistemas embarcados.

Sinta-se livre para utilizar, modificar e contribuir com melhorias.

---

# Autor

**Paulo Cesar Furlanetto Marques**

Especialista em:

- ESP32
- Raspberry Pi
- PostgreSQL
- C#
- Sistemas Embarcados
- Redes e IoT

---

⭐ Se este projeto foi útil para você, considere deixar uma estrela no repositório.
