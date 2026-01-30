# CBDC Wallet - AI-Driven Microfinance Research Prototype

**Project URL**: [https://ucxdhrikpgxgzbdkwzfc.supabase.co](https://ucxdhrikpgxgzbdkwzfc.supabase.co)

---

## 📄 Abstract
This application investigates the evolving role of Financial Technology (FinTech) in shaping the modern digital economy, with a focus on developing countries such as **Sri Lanka**. It explores blockchain applications beyond cryptocurrencies, Central Bank Digital Currencies (CBDCs), Artificial Intelligence (AI) for microfinance, and Open Banking APIs. Using a systematic literature review and technical prototyping, the study identifies key trends and proposes a technical framework for AI-driven credit scoring that utilizes alternative data to bridge the financial inclusion gap.

---

## 🚀 Research Methodology & AI Framework
The proposed methodology moves from static assessment to a dynamic, high-dimensional non-linear analysis of alternative data.

### **1. The Scoring Architecture**
*   **Feature Vector ($X$)**: We define the digital footprint of a rural borrower as a vector:
    $$X = \{w_1x_{telco}, w_2x_{utility}, w_3x_{wallet}, w_4x_{social}\}$$
*   **Probability of Default ($PD$)**: The likelihood of default is calculated using the Sigmoid function:
    $$PD = \frac{1}{1 + \exp(-(\beta_0 + \sum_{i=1}^{n} \beta_i x_i))}$$
*   **Standardized Score ($S$)**: To align with the **Credit Information Bureau (CRIB) of Sri Lanka**, the score is scaled using Points to Double the Odds (PDO) logic:
    $$S = Offset + Factor \times \ln\left(\frac{1 - PD}{PD}\right)$$
*   **Recovery and Rehabilitation Algorithm ($RI$)**: The system implements a Recovery Index to facilitate rehabilitation for seasonal volatility:
    $$RI(t) = RI_{base} \cdot (1 - e^{-\lambda \Delta t}) + \sum_{j=1}^{k} \gamma_j \Delta C_j$$

---

## 📊 Traditional CRIB vs. AI Framework

| Feature | Traditional CRIB (Sri Lanka) | Proposed AI-Alternative Scoring |
| :--- | :--- | :--- |
| **Data Source** | Formal Bank Collateral | Utility, CBDC Velocity, Telco |
| **Accessibility** | Banked Population Only | Inclusive for Rural MSMEs |
| **Update Cycle** | Monthly/Quarterly | Real-Time / Dynamic |
| **Recovery Path** | Static (Years) | Mathematical Rehabilitation (RI) |

---

## 📡 100% Free Deployment Guide
...

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