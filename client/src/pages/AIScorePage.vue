<template>
  <q-page class="bg-dark-bg q-pa-lg">
    <div class="aiscore-content fade-in-up">
      <div class="text-center q-mb-xl">
        <h1 class="text-h2 text-weight-bolder text-gradient q-mb-md">AI Insights & Scoring</h1>
        <p class="text-grey-5 text-h6 text-weight-light">Machine learning analytics for your enterprise digital assets</p>
      </div>

      <div class="row q-col-gutter-lg">
        <!-- Main Score Card -->
        <div class="col-12 col-md-4">
          <div class="wallet-card text-center q-pa-xl shadow-24 flex flex-col items-center justify-center">
            <div class="score-circle q-mb-lg flex flex-center">
              <div>
                <div class="text-h1 text-weight-bolder text-white">742</div>
                <div class="text-subtitle1 text-grey-5 font-bold">EXCELLENT</div>
              </div>
            </div>
            <p class="text-grey-4 q-px-md">Your score has increased by <strong>12 points</strong> since your last assessment.</p>
            <q-btn label="Recalculate Score" class="btn-primary q-px-lg q-mt-md" rounded no-caps unelevated />
          </div>
        </div>

        <!-- Breakdown -->
        <div class="col-12 col-md-8">
          <div class="wallet-card q-pa-lg shadow-24 full-height">
            <h3 class="text-h5 text-white text-weight-bold q-mb-lg">Scoring Breakdown</h3>
            <div class="q-gutter-y-xl">
              <div v-for="item in breakdown" :key="item.label">
                <div class="row items-center justify-between q-mb-sm">
                  <div class="row items-center">
                    <q-icon :name="item.icon" color="primary" size="24px" class="q-mr-sm" />
                    <span class="text-subtitle1 text-white">{{ item.label }}</span>
                  </div>
                  <span class="text-subtitle1 text-weight-bold" :class="item.color">{{ item.score }}/100</span>
                </div>
                <q-linear-progress :value="item.score / 100" :color="item.color.split('-')[1]" rounded size="10px" />
                <p class="text-caption text-grey-5 q-mt-sm">{{ item.desc }}</p>
              </div>
            </div>
          </div>
        </div>

        <!-- AI Recommendations -->
        <div class="col-12">
          <div class="wallet-card q-pa-lg shadow-24">
            <h3 class="text-h5 text-white text-weight-bold q-mb-lg">AI Recommendations</h3>
            <div class="row q-col-gutter-md">
              <div class="col-12 col-md-4" v-for="rec in recommendations" :key="rec.title">
                <div class="glass-effect q-pa-md border-all" style="border-radius: 16px;">
                  <div class="row items-center q-mb-md">
                    <q-icon :name="rec.icon" :color="rec.color" size="28px" class="q-mr-sm" />
                    <h4 class="text-subtitle1 text-white text-weight-bold q-my-none">{{ rec.title }}</h4>
                  </div>
                  <p class="text-grey-5 text-caption">{{ rec.desc }}</p>
                  <q-btn flat :color="rec.color" label="Apply Suggestion" no-caps dense />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </q-page>
</template>

<script setup>
const breakdown = [
  { icon: 'history', label: 'Transaction History', score: 88, color: 'text-green-4', desc: 'Consistency in wallet interactions and liquidity management.' },
  { icon: 'verified_user', label: 'Wallet Security', score: 95, color: 'text-green-5', desc: 'Advanced MFA enabled and secure transaction patterns detected.' },
  { icon: 'balance', label: 'Asset Stability', score: 62, color: 'text-amber-5', desc: 'Asset volatility exposure is slightly higher than optimized levels.' }
]

const recommendations = [
  { icon: 'swap_horiz', title: 'Optimize Liquidity', desc: 'Transfer 500 CBDC to your reserve vault to improve your stability score.', color: 'primary' },
  { icon: 'security', title: 'Update Protocol', desc: 'Your current smart contract interaction version is legacy. Update to v2.4.', color: 'purple-4' },
  { icon: 'trending_up', title: 'Staking Opportunity', desc: 'Based on your holding patterns, you could earn 4% APY by staking IOTA.', color: 'teal-4' }
]
</script>

<style scoped lang="scss">
.aiscore-content {
  max-width: 1200px;
  margin: 0 auto;
}

.score-circle {
  width: 220px;
  height: 220px;
  border-radius: 50%;
  border: 15px solid rgba(102, 126, 234, 0.1);
  border-top-color: #667eea;
  border-right-color: #764ba2;
  position: relative;
  box-shadow: 0 0 30px rgba(102, 126, 234, 0.2);

  &::after {
    content: '';
    position: absolute;
    top: -15px; left: -15px; right: -15px; bottom: -15px;
    border-radius: 50%;
    border: 2px solid rgba(255, 255, 255, 0.05);
  }
}

.bg-dark-bg {
  background: #0a0e27;
}

.border-all {
  border: 1px solid rgba(255, 255, 255, 0.05);
}
</style>
