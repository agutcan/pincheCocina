# 🍳 pincheCocina – Smart Recipe Assistant

**pincheCocina** es una aplicación móvil multiplataforma desarrollada con **.NET MAUI** que redefine la gestión de recetas mediante un enfoque híbrido y manos libres.  
Permite a los usuarios crear, editar y consultar recetas utilizando **procesamiento de lenguaje natural** (*Speech-to-Text*) y **síntesis de voz** (*Text-to-Speech*), facilitando la interacción mientras se cocina.

---

## 💎 Propuesta de Valor

A diferencia de un recetario estático, **pincheCocina** actúa como un asistente activo:

- **Entrada Inteligente**  
  No necesitas escribir; dictas la receta y el sistema extrae la lógica.

- **Lectura Dinámica**  
  La app te guía en la cocina leyendo los pasos y traduciendo unidades técnicas a lenguaje natural.

- **Arquitectura Profesional**  
  Implementación estricta de **MVVM** y **Clean Architecture** para garantizar escalabilidad y mantenibilidad.

---

## 🛠️ Stack Tecnológico

- **Framework Principal:** .NET MAUI 8.0 (Android, iOS, Windows)
- **Lenguaje:** C# 12
- **ORM / Persistencia:** Entity Framework Core + SQLite
- **Procesamiento de Texto:** Motores de Regex personalizados para parsing de ingredientes y tiempos
- **Automatización de UI:** Fody.PropertyChanged para la gestión de estados y notificaciones
- **Servicios Nativos:**
  - CommunityToolkit.Maui (Speech-to-Text)
  - MAUI Essentials (Text-to-Speech)
- **Calidad:** MSTest & Moq para Unit Testing

---

## 🏗️ Arquitectura y Diseño de Software

El proyecto sigue el patrón **MVVM**, separando claramente las responsabilidades para facilitar el mantenimiento y la testabilidad.

### 1️⃣ Capa de Modelos (Domain)

Define entidades con relaciones uno a muchos:

- **Receta** 1 ➔ N **PasoReceta**
- **PasoReceta** 1 ➔ N **Ingrediente**

---

### 2️⃣ Capa de Datos y Servicios

- **AppDbContext**
  - Gestiona la base de datos SQLite
  - Incluye lógica de *Seeding* para datos iniciales

- **RecetaService**
  - Implementa `IRecetaService`
  - Maneja el ciclo de vida de los datos (CRUD)
  - Incluye una solución avanzada para el rastreo de entidades de EF Core (*Detached State*) para evitar conflictos en actualizaciones complejas

---

### 3️⃣ Motores de Procesamiento (ViewModel Logic)

El corazón de la app reside en `CrearRecetaViewModel`.  
El método **ProcesarTextoDictado** realiza:

- **Normalización**  
  Convierte números en texto (`"dos"`) a enteros (`"2"`)

- **Tokenización**  
  Divide el discurso en pasos usando conectores como:
  - `luego`
  - `después`
  - `y por último`

- **Extracción de Tiempos**  
  Detecta patrones de minutos y horas con conversión automática  
  `1h ➔ 60 min`

- **Extracción de Ingredientes**  
  Identifica **Cantidad**, **Unidad** y **Nombre** mediante un motor de Regex robusto

---

## 📱 Interfaz de Usuario (UI/UX)

La aplicación ofrece **dos modos de interacción** mediante `DataTriggers` en XAML:

### 🎙️ Modo Micro
- Enfoque en el dictado por voz
- Feedback visual de **"Grabando"**
- Procesamiento automático de párrafos largos
- Ideal para cocinar sin usar las manos

### ✍️ Modo Mano
- Interfaz tradicional con campos de texto
- Control granular sobre cada paso
- Ideal para correcciones rápidas

---

## 🔊 Funcionalidades de Voz Avanzadas

### Conversión de Unidades para Lectura (TTS)

Para evitar una lectura robótica, el sistema traduce abreviaturas de base de datos a lenguaje natural:

- `250 gr` ➔ *"Doscientos cincuenta gramos"*
- `1 pza` ➔ *"Una pieza"*
- `500 ml` ➔ *"Quinientos mililitros"*

---

### Ejemplo de Procesamiento Regex

Para capturar ingredientes como:

> **"500 gr de harina de trigo"**

Se utiliza el siguiente patrón:

```regex
(\d+(?:[\.,]\d+)?)\s*(gramos|gr|g|ml|l|piezas)?\s*(?:de\s+)?([a-záéíóúñ\s]+)
```

---

## 🧪 Calidad y Pruebas

El proyecto incluye un **proyecto de pruebas unitarias independiente** (`TestProject1`) enfocado en validar la lógica de negocio sin depender de la interfaz de usuario ni de servicios nativos.

### Tipos de Pruebas Implementadas

- **Pruebas de Parsing de Texto**  
  Garantizan que las instrucciones dictadas se transformen correctamente en datos estructurados.

  Ejemplo:
  - Entrada: `"Cocinar por 15 minutos"`
  - Resultado esperado: `TiempoMinutos = 15`

- **Pruebas de Conversión de Unidades de Tiempo**  
  Verifican la correcta normalización de expresiones temporales.

  Ejemplo:
  - `"1 hora"` ➔ `60 minutos`

- **Pruebas de Extracción de Ingredientes**  
  Validan que el motor de Regex identifique correctamente:
  - Cantidad
  - Unidad
  - Nombre del ingrediente

- **Mocks de Servicios**  
  Uso de **Moq** para simular el acceso a datos y probar los ViewModels de forma aislada, garantizando:
  - Alta testabilidad
  - Bajo acoplamiento
  - Pruebas determinísticas

### Herramientas de Testing

- **Framework de pruebas:** MSTest
- **Mocking:** Moq
- **Enfoque:** Unit Testing orientado a lógica de negocio

---

## ⚙️ Configuración del Entorno

### 1️⃣ Clonar el Repositorio

```bash
git clone [url-del-repo]
```

### 2️⃣ Instalar Workloads de .NET MAUI

Antes de compilar el proyecto, asegúrate de tener instalado el workload de **.NET MAUI**:

```bash
dotnet workload install maui
```

### 3️⃣ Clonar el Repositorio

Clona el repositorio del proyecto en tu máquina local:

```bash
git clone [url-del-repo]
```

Accede al directorio del proyecto:

```bash
cd pincheCocina
```

### 4️⃣ Restaurar Paquetes NuGet

Restaura todas las dependencias necesarias del proyecto:

```bash
dotnet restore
```

Paquetes principales utilizados:

- CommunityToolkit.Maui
- EntityFrameworkCore.Sqlite
- Fody.PropertyChanged
- MSTest
- Moq

### 5️⃣ Ejecutar la Aplicación

1. Abre la solución en **Visual Studio**
2. Selecciona el target deseado:
   - **Android**
   - **Windows**
   - **iOS** (requiere macOS)
3. Presiona **F5** para compilar y ejecutar la aplicación

---

### 🧩 Consideraciones por Plataforma

#### Android
- Requiere Android SDK instalado
- Emulador o dispositivo físico recomendado para pruebas

#### Windows
- Compatible con Windows 10 / 11
- Utiliza **WinUI 3** como backend de UI

#### iOS
- Requiere macOS con Xcode instalado
- Compilación mediante Mac local o remoto

---

### 📝 Notas Técnicas

- La base de datos **SQLite** se crea automáticamente en el primer arranque.
- El *Seeding* inicial permite comenzar a usar la app sin crear recetas desde cero.
- La arquitectura desacoplada facilita el mantenimiento y la evolución del proyecto.

---

# IMPORTANTE!!
Ejemplos funcionales de crear receta con audio:
- // Poner a hervir 2 litros de agua luego añadir 500 gramos de macarrones y cocinar por 10 minutos y por último escurrir y servir.
- // Mezclar 250 g de harina con 2 piezas de huevo después añadir 100 g de mantequilla y batir por 5 minutos
- // Añadir 250 ml de leche y 1 l de caldo siguiente paso cocinar a fuego lento por 15 minutos
