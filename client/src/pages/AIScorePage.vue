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
                <div v-if="loading" class="text-subtitle1 text-grey-5">...</div>
                <div v-else class="text-h1 text-weight-bolder text-white">{{ creditScore }}</div>
                <div class="text-subtitle1 text-grey-5 font-bold">{{ rating }}</div>
              </div>
            </div>
            <p v-if="creditScore > 0" class="text-grey-4 q-px-md">Your AI-calculated sovereign credit reliability is verified.</p>
            <p v-else class="text-grey-4 q-px-md">Our AI Networker is currently analyzing your digital footprint. Check back in a few seconds.</p>
            <q-btn label="Refresh Data" @click="refreshData" class="btn-primary q-px-lg q-mt-md" rounded no-caps unelevated :loading="loading" />
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
import { ref, onMounted } from 'vue'
import { getUser } from 'src/api'

const creditScore = ref(0)
const rating = ref('PENDING')
const loading = ref(true)

const breakdown = ref([
  { icon: 'history', label: 'Transaction History', score: 0, color: 'text-green-4', desc: 'Consistency in wallet interactions and liquidity management.' },
  { icon: 'verified_user', label: 'Network Integrity', score: 0, color: 'text-green-5', desc: 'Verification status and telco/utility data consistency.' },
  { icon: 'balance', label: 'Account Health', score: 0, color: 'text-amber-5', desc: 'Balance maintenance and recovery indicators.' }
])

const recommendations = [
  { icon: 'swap_horiz', title: 'Optimize Liquidity', desc: 'Maintain a minimum balance of 500 CBDC to improve your stability score.', color: 'primary' },
  { icon: 'security', title: 'Update Protocol', desc: 'Ensure your National ID is verified to reach the Platinum scoring tier.', color: 'purple-4' },
  { icon: 'trending_up', title: 'Transaction Velocity', desc: 'Regular weekly transactions can increase your score by up to 25 points.', color: 'teal-4' }
]

const refreshData = async () => {
  loading.value = true
  const userJson = localStorage.getItem('cbdc_user')
  if (userJson) {
    const localUser = JSON.parse(userJson)
    try {
      const freshUser = await getUser(localUser.id)
      if (freshUser && freshUser.id) {
        creditScore.value = freshUser.creditScore || 0

        // Update local storage with fresh data
        localStorage.setItem('cbdc_user', JSON.stringify(freshUser))

        // Determine Rating
        if (creditScore.value > 750) rating.value = 'EXCELLENT'
        else if (creditScore.value > 650) rating.value = 'GOOD'
        else if (creditScore.value > 500) rating.value = 'FAIR'
        else if (creditScore.value > 0) rating.value = 'POOR'
        else rating.value = 'CALCULATING...'

        // Mock some breakdown scores based on real total
        breakdown.value[0].score = Math.min(100, Math.floor(creditScore.value / 10) + 15)
        breakdown.value[1].score = Math.min(100, Math.floor(creditScore.value / 12) + 20)
        breakdown.value[2].score = Math.min(100, Math.floor(creditScore.value / 15) + 30)
      }
    } catch (error) {
      console.error('Error fetching fresh user data:', error)
      creditScore.value = localUser.creditScore || 0
    }
  }
  loading.value = false
}

onMounted(refreshData)
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
