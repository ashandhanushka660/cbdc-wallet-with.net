<template>
  <q-page class="bg-dark-bg flex flex-center overflow-hidden">
    <!-- Background Animated Shapes -->
    <div class="absolute-full overflow-hidden no-pointer-events">
      <div class="shape-1 animate-blob"></div>
      <div class="shape-2 animate-blob animation-delay-2000"></div>
    </div>

    <div class="container q-pa-xl text-center z-top">
      <div class="row items-center justify-center q-mb-xl">
        <q-icon name="account_balance_wallet" size="80px" class="text-gradient q-mr-md shadow-20" />
        <h1 class="text-h1 text-weight-bolder text-white q-my-none">CBDC</h1>
      </div>

      <div class="row justify-center q-mb-xl">
        <div class="col-12 col-md-8">
          <h2 class="text-h2 text-weight-bold text-gradient-light q-mb-md">
            The Future of Enterprise Digital Assets
          </h2>
          <p class="text-h5 text-grey-5 text-weight-light line-height-1-6">
            Secure, scalable, and intelligent. Experience the most advanced 3-tier digital wallet architecture designed for institutional-grade stability. Powered by Azure SQL.
          </p>
        </div>
      </div>

      <div class="row justify-center q-gutter-md q-pt-lg">
        <q-btn
          to="/register"
          label="Get Started Now"
          class="btn-primary q-px-xl q-py-md text-h6 text-weight-bold"
          no-caps
          unelevated
          rounded
        />
        <q-btn
          to="/login"
          label="Login to Wallet"
          class="btn-secondary q-px-xl q-py-md text-h6 text-weight-bold"
          no-caps
          outline
          rounded
        />
      </div>

      <!-- Feature Grid -->
      <div class="row q-col-gutter-xl q-mt-xl pt-100">
        <div class="col-12 col-md-4" v-for="f in features" :key="f.title">
          <div class="feature-card q-pa-xl shadow-24">
            <q-icon :name="f.icon" size="48px" class="text-gradient q-mb-lg" />
            <h3 class="text-h5 text-white text-weight-bold q-mb-md">{{ f.title }}</h3>
            <p class="text-grey-5 text-weight-light">{{ f.desc }}</p>
          </div>
        </div>
      </div>
    </div>
  </q-page>
</template>

<script setup>
const features = [
  { icon: 'security', title: 'Bank-Grade Security', desc: 'Enterprise-level encryption and 3-tier architecture ensuring your assets are always safe.' },
  { icon: 'bolt', title: 'Instant Settlement', desc: 'Experience lightning-fast transactions with direct CBDC network integration.' },
  { icon: 'auto_awesome', title: 'AI Scoring', desc: 'Intelligent credit scoring and risk assessment powered by advanced machine learning.' }
]
</script>

<style scoped lang="scss">
.bg-dark-bg {
  background: #0a0e27;
}

.text-gradient-light {
  background: linear-gradient(135deg, #ffffff 0%, #a5b4fc 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.container {
  max-width: 1200px;
}

.line-height-1-6 {
  line-height: 1.6;
}

.pt-100 {
  padding-top: 100px;
}

.feature-card {
  background: rgba(255, 255, 255, 0.03);
  border-radius: 24px;
  border: 1px solid rgba(255, 255, 255, 0.05);
  transition: all 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
  height: 100%;

  &:hover {
    transform: translateY(-10px);
    background: rgba(255, 255, 255, 0.05);
    border-color: rgba(102, 126, 234, 0.3);
    box-shadow: 0 20px 40px rgba(0, 0, 0, 0.4);
  }
}

// Background Blobs
.shape-1 {
  position: absolute;
  top: -10%;
  left: -10%;
  width: 500px;
  height: 500px;
  background: radial-gradient(circle, rgba(102, 126, 234, 0.15) 0%, transparent 70%);
  filter: blur(80px);
}

.shape-2 {
  position: absolute;
  bottom: -10%;
  right: -10%;
  width: 600px;
  height: 600px;
  background: radial-gradient(circle, rgba(118, 75, 162, 0.15) 0%, transparent 70%);
  filter: blur(80px);
}

@keyframes blob {
  0% { transform: translate(0px, 0px) scale(1); }
  33% { transform: translate(30px, -50px) scale(1.1); }
  66% { transform: translate(-20px, 20px) scale(0.9); }
  100% { transform: translate(0px, 0px) scale(1); }
}

.animate-blob {
  animation: blob 7s infinite;
}

.animation-delay-2000 {
  animation-delay: 2s;
}
</style>
