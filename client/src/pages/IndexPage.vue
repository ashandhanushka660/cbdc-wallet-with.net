<template>
  <q-page class="bg-dark text-white overflow-hidden">
    <!-- 1. Hero Section -->
    <section class="min-h-screen flex flex-center relative-position q-px-md">
      <!-- Dynamic Background -->
      <div class="absolute-full gradient-bg"></div>
      <div class="absolute-full bg-noise opacity-05"></div>

      <div class="container text-center relative-position z-10 q-pb-xl">
        <div style="height: 10vh"></div>
        <div class="text-overline text-primary q-mb-sm text-weight-bolder letter-spacing-3 opacity-80">
          THE INTERMEDIARY STANDARD
        </div>
        <h1 class="text-h2 md:text-h1 text-weight-bolder leading-tight q-mb-lg hero-title">
          Bridging Central Banks <br class="gt-xs" />
          <span class="text-primary-gradient">to the Modern Economy</span>
        </h1>
        <p class="text-h6 text-grey-4 q-px-xl q-mx-auto max-w-700 leading-relaxed text-weight-light">
          The sovereign digital wallet intermediary. Secure, offline-capable, and integrated
          directly with the two-tier CBDC architecture.
        </p>

        <div class="row q-gutter-md justify-center q-mt-xl q-mb-xl">
          <q-btn
            unelevated
            label="Launch Wallet"
            to="/auth/register"
            class="bg-primary-gradient text-white q-px-xl text-weight-bold shadow-glow q-py-md"
            rounded
            no-caps
          />
          <q-btn
            outline
            color="white"
            label="Documentation"
            to="/documentation"
            no-caps
            rounded
            class="q-px-xl q-py-md"
            icon-right="menu_book"
          />
        </div>

        <!-- Devices Preview -->
        <div class="row justify-center q-mt-xl">
          <div class="mockup-glass q-pa-lg">
            <div class="row items-center justify-center text-grey-5">
              <q-icon name="devices" size="48px" class="q-mr-md" />
              <div class="text-left">
                <div class="text-weight-bold text-white">Full PWA Support</div>
                <div class="text-caption">iOS • Android • Desktop</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- 2. Features Section -->
    <section class="q-py-xl container">
      <div class="text-center q-mb-xl">
        <h2 class="text-h4 md:text-h3 text-weight-bold">Fintech-Grade Capabilities</h2>
        <div class="h-1 bg-primary-gradient w-24 mx-auto q-mt-md rounded" style="height: 4px; width: 100px;"></div>
      </div>

      <div class="row q-col-gutter-lg">
        <div class="col-12 col-md-4">
          <q-card flat class="glass-card full-height q-pa-lg">
            <q-card-section>
              <div class="icon-box bg-accent-soft q-mb-md">
                <q-icon name="wifi_off" size="32px" class="text-accent" />
              </div>
              <div class="text-h5 text-weight-bold q-mb-sm">Offline-First</div>
              <div class="text-grey-4 text-body1">
                Transact beyond limits. Balance and history are cached locally, syncing
                automatically when connectivity returns.
              </div>
            </q-card-section>
          </q-card>
        </div>

        <div class="col-12 col-md-4">
          <q-card flat class="glass-card full-height q-pa-lg">
            <q-card-section>
              <div class="icon-box bg-warning-soft q-mb-md">
                <q-icon name="bolt" size="32px" class="text-warning" />
              </div>
              <div class="text-h5 text-weight-bold q-mb-sm">Instant Ledger</div>
              <div class="text-grey-4 text-body1">
                Experience instant finality. Built on enterprise architecture for sub-second transaction
                updates.
              </div>
            </q-card-section>
          </q-card>
        </div>

        <div class="col-12 col-md-4">
          <q-card flat class="glass-card full-height q-pa-lg">
            <q-card-section>
              <div class="icon-box bg-success-soft q-mb-md">
                <q-icon name="fingerprint" size="32px" class="text-green-4" />
              </div>
              <div class="text-h5 text-weight-bold q-mb-sm">Biometric Security</div>
              <div class="text-grey-4 text-body1">
                Bank-grade protection. Integrates device biometrics and modern MFA for
                multi-factor verification.
              </div>
            </q-card-section>
          </q-card>
        </div>
      </div>
    </section>

    <!-- 3. AI Microfinance Section -->
    <section class="q-py-xl container">
      <q-card flat class="glass-card q-pa-xl">
        <div class="row items-center q-col-gutter-xl">
          <div class="col-12 col-md-8">
            <h2 class="text-h3 text-weight-bolder text-primary q-mb-md">AI Microfinance</h2>
            <p class="text-h6 text-grey-4 leading-relaxed">
              Empowering rural economies with AI-driven credit scoring and instant CBDC micro-loans. 
              Bridging the gap in financial inclusion through decentralized credibility assessment.
            </p>
          </div>
          <div class="col-12 col-md-4 text-center">
            <div class="relative-position">
              <q-icon name="psychology" size="140px" color="primary" class="opacity-10" />
              <q-icon name="auto_awesome" size="48px" color="primary" class="absolute-center" />
            </div>
          </div>
        </div>
      </q-card>
    </section>
  </q-page>
</template>

<style lang="scss" scoped>
.min-h-screen { min-height: 100vh; }
.container { max-width: 1200px; margin: 0 auto; }
.z-10 { z-index: 10; }

.gradient-bg {
  background: radial-gradient(circle at 50% -20%, #1e3a8a 0%, #000 70%);
}

.text-primary-gradient {
  background: linear-gradient(135deg, #00d2ff 0%, #3a7bd5 100%);
  -webkit-background-clip: text;
  background-clip: text;
  -webkit-text-fill-color: transparent;
}

.bg-primary-gradient {
  background: linear-gradient(135deg, #00d2ff 0%, #3a7bd5 100%);
}

.glass-card {
  background: rgba(255, 255, 255, 0.03);
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 16px;
  transition: all 0.4s ease;
  &:hover {
    transform: translateY(-8px);
    background: rgba(255, 255, 255, 0.06);
    border-color: rgba(0, 210, 255, 0.3);
  }
}

.mockup-glass {
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 20px;
  width: 300px;
}

.icon-box {
  width: 60px;
  height: 60px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.bg-accent-soft { background: rgba(0, 210, 255, 0.1); }
.text-accent { color: #00d2ff; }
.bg-warning-soft { background: rgba(242, 192, 55, 0.1); }
.bg-success-soft { background: rgba(76, 175, 80, 0.1); }

.shadow-glow {
  box-shadow: 0 4px 20px rgba(0, 210, 255, 0.4);
}

.leading-tight { line-height: 1.1; }
.leading-relaxed { line-height: 1.6; }
.letter-spacing-3 { letter-spacing: 3px; }
</style>
