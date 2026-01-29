# CBDC Wallet - Enterprise Institutional Digital Wallet

A premium, 3-tier sovereign digital wallet system engineered for central bank digital currency (CBDC) ecosystems. Built with a high-performance .NET 8 backend and a visually stunning Quasar (Vue 3) frontend.

---

## 🚀 Vision & Architecture

This application simulates an institutional-grade intermediary wallet, bridging the gap between central banks and the modern economy with a focus on:
- **Sovereign Security**: Identity verification and atomic finality.
- **AI-Driven Analytics**: Intelligent credit scoring and micro-loan assessments.
- **Enterprise Reliability**: Built on .NET 8 Minimal APIs for sub-millisecond responsiveness.

### 🏢 3-Tier Enterprise Stack
- **Frontend**: Quasar Framework + Vite (Institutional Dark Theme, Glassmorphism)
- **Backend API**: .NET 8 Web API (Clean Architecture, Minimal APIs)
- **Ledger Storage**: SQL Server / Azure SQL (Relational integrity and auditability)

---

## 📊 Enterprise Gap Analysis

| Feature Category | Current Implementation (v2.1) | Enterprise Institutional Requirement |
| :--- | :--- | :--- |
| **Authentication** | Session + LocalStorage State | Secure JWT w/ Refresh Tokens, RBAC, OAuth2/OIDC |
| **Data Integrity** | Transaction Ledger Table | Immutable Audit Trail, Merkle Tree Proofs |
| **Security** | Basic Hashing | AES-256 Encryption, Azure Key Vault / HSM, MFA/2FA |
| **Compliance** | Identity Verification Form | KYC/AML Live Monitoring, Regulatory Reporting Hooks |
| **Reliability** | Development Server | Dockerized Microservices, Health Checks, HA Deployments |
| **AI/ML** | Rule-based Score Assessment | Real-time ML Pipeline (Python/ONNX) for Risk Analysis |

---

## 🎯 Institutional Features (Implemented)

✅ **Institutional Landing Portal**: High-end entrance with feature highlighting and animated backgrounds.
✅ **Sovereign Dashboard**: Central nerve center with **Real-time Asset Tickers**.
✅ **AI Credit Index**: Visual score meter with risk-tiering and micro-loan application logic.
✅ **Institutional Settings**: Profile management and Infrastructure security controls (MFA).
✅ **Enterprise Backend**: Health Checks (`/health`), Global Error Handling, and Swagger documentation.
✅ **Identity Verification**: Professional NID-based registration flow.

---

## 📁 Project Layout

```
cbdcwalletenterprise/
├── client/          # Quasar frontend (Vue.js 3 + Vite)
│   ├── src/pages/   # Index, Register, Dashboard, Loans
│   └── src/layouts/ # Institutional & Dashboard layouts
└── server/          # .NET 8 Backend API
    ├── Data/        # AppDbContext & Migrations
    ├── Models/      # User, Wallet, Transaction, Loan
    └── Program.cs   # API Endpoints & Service Configuration
```

---

## 🔧 Deployment & Hosting

### 🚀 Deploying to Vercel (Frontend)
1. **Build**: `npm run build` from the `client/` directory.
2. **Deploy**: Push to GitHub and connect to Vercel.
3. **Configuration**: Use `quasar.config.js` for SPA mode settings.

### ☁️ Deploying to Azure (Backend)
1. **Publish**: `dotnet publish -c Release`
2. **Database**: Provision an Azure SQL instance.
3. **Env Vars**: Update `ConnectionStrings:DefaultConnection` in Azure App Service settings.

---

## 🔧 Institutional Setup

### Prerequisites
- Node.js 18+ & .NET 8 SDK
- SQL Server LocalDB (Instance: `(localdb)\MSSQLLocalDB`)

### Execution
1. **Ledger Server**:
   ```bash
   cd server
   dotnet build
   dotnet run --urls "http://localhost:5005"
   ```
2. **Client Portal**:
   ```bash
   cd client
   npm install
   npm run dev
   ```

---

## 📄 Compliance & License

Licensed for institutional research and development. 

**Built with ❤️ by Antigravity AI for Sovereign Digital Economies.**