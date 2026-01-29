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
- **Ledger Storage**: PostgreSQL / Supabase (Relational integrity and auditability)

---

## 📊 Enterprise Gap Analysis

| Feature Category | Current Implementation (v2.5) | Enterprise Institutional Requirement |
| :--- | :--- | :--- |
| **Authentication** | Session + LocalStorage State | Secure JWT w/ Refresh Tokens, RBAC, OAuth2/OIDC |
| **Data Integrity** | Transaction Ledger Table | Immutable Audit Trail, Merkle Tree Proofs |
| **Security** | Basic Hashing | AES-256 Encryption, Azure Key Vault / HSM, MFA/2FA |
| **Compliance** | Identity Verification Form | KYC/AML Live Monitoring, Regulatory Reporting Hooks |
| **Reliability** | Dockerized + Health Checks | Microservices, HA Deployments, Kubernetes |
| **AI/ML** | Rule-based Score Assessment | Real-time ML Pipeline (Python/ONNX) for Risk Analysis |

---

## 🎯 Institutional Features (Implemented)

✅ **Institutional Landing Portal**: High-end entrance with feature highlighting and animated backgrounds.
✅ **Sovereign Dashboard**: Central nerve center with **Real-time Asset Tickers**.
✅ **Governance Command Center**: Admin portal for ledger and AI score management.
✅ **Ecosystem Services**: Functional directory for utility, tax, and pension hubs.
✅ **AI Credit Index**: Visual score meter with risk-tiering and micro-loan application logic.
✅ **Institutional Settings**: Profile management and Infrastructure security controls (MFA).
✅ **Enterprise Backend**: Health Checks (`/health`), Global Error Handling, and Swagger documentation.
✅ **Identity Verification**: Professional NID-based registration flow.

---

## 📡 100% Free Deployment Guide

This project is engineered for **Zero-Cost Sovereign Hosting** using the industry's best free tiers.

### **1. Database (Supabase)**
*   **Provider**: [Supabase.com](https://supabase.com) (PostgreSQL)
*   **Cost**: $0 (Free Tier)
*   **Setup**: 
    1. Create a project on Supabase.
    2. Go to **Project Settings** -> **Database**.
    3. Copy the **URI** connection string.
    4. You will use this as `DATABASE_URL` in the next step.

### **2. Backend API (Render)**
*   **Provider**: [Render.com](https://render.com) (Web Service)
*   **Cost**: $0 (Free Tier)
*   **Setup**:
    1. Click **New** -> **Web Service**.
    2. Connect your GitHub repository.
    3. **Runtime**: Select `Docker`.
    4. **Root Directory**: `server`.
    5. **Environment Variables**:
       * `DATABASE_URL`: Your Supabase URI from Step 1.
    6. Click **Deploy**.

### **3. Frontend Portal (Vercel)**
*   **Provider**: [Vercel.com](https://vercel.com)
*   **Cost**: $0 (Hobby Tier)
*   **Setup**:
    1. Import the repository and select `client` as Root.
    2. Set **Build Command**: `npm run build`.
    3. Set **Output Directory**: `dist/spa`.
    4. **Environment Variables**:
       * `VITE_API_URL`: Your new Render URL.

---

### **Current Live Links**
*   **Production Frontend**: [https://client-4whjirqps-dhanuashans-projects.vercel.app](https://client-4whjirqps-dhanuashans-projects.vercel.app)
*   **Code Repository**: [https://github.com/ashandhanushka660/cbdc-wallet-with.net](https://github.com/ashandhanushka660/cbdc-wallet-with.net)

---

**Built with ❤️ by Antigravity AI for Sovereign Digital Economies.**