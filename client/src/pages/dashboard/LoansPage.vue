<template>
  <q-page class="q-pa-lg bg-dark text-white">
    <div class="row q-col-gutter-lg">
      <!-- AI Credit Scoring -->
      <div class="col-12 col-md-4">
        <q-card dark class="glass-card q-pa-lg text-center shadow-glow">
          <div class="text-overline text-primary q-mb-md">AI CREDIT PROFILE</div>
          <div class="row justify-center q-mb-lg">
             <div class="score-display flex flex-center" :style="{ borderColor: getScoreColor(creditScore) }">
                <div>
                    <div class="text-h2 text-weight-bolder">{{ creditScore }}</div>
                    <div class="text-caption text-weight-bold opacity-70">Sovereign Index</div>
                </div>
             </div>
          </div>

          <div class="text-h6 text-weight-bold q-mb-sm">Risk Tier: <span :style="{ color: getScoreColor(creditScore) }">{{ riskTier }}</span></div>
          <p class="text-grey-5 q-px-md">Your credit profile is assessed in real-time based on atomic transaction patterns and liquidity ratios.</p>

          <q-btn outline color="white" label="Recalculate AI Index" class="q-mt-md" @click="refreshScore" :loading="loading" no-caps rounded />
        </q-card>
      </div>

      <!-- Active Loans -->
      <div class="col-12 col-md-8">
        <q-card dark class="bg-surface q-pa-md height-100 rounded-borders">
          <q-card-section class="row items-center justify-between">
            <div class="text-h6 text-weight-bold">Sovereign Micro-Loans</div>
            <q-btn color="primary" label="New Application" icon="add" @click="showApplyDialog = true" no-caps rounded unelevated />
          </q-card-section>

          <q-separator dark />

          <q-card-section>
            <div v-if="loans.length > 0">
               <q-list dark separator>
                  <q-item v-for="loan in loans" :key="loan.id" class="q-py-md">
                    <q-item-section avatar>
                      <q-avatar color="primary" text-color="white" icon="account_balance" />
                    </q-item-section>
                    <q-item-section>
                      <q-item-label class="text-weight-bold">${{ loan.remaining_balance }} Remaining</q-item-label>
                      <q-item-label caption class="text-grey-4">
                        Original: ${{ loan.principal }} • {{ loan.rate }}% APR
                      </q-item-label>
                    </q-item-section>
                    <q-item-section side>
                       <q-chip color="green-9" text-color="white" size="sm" class="text-weight-bold">ACTIVE</q-chip>
                       <div class="text-caption text-grey-6">Next: {{ loan.next_payment }}</div>
                    </q-item-section>
                  </q-item>
               </q-list>
            </div>
            <div v-else class="text-center q-pa-xl text-grey-8">
              <q-icon name="payments" size="64px" class="q-mb-md" />
              <div class="text-h6">No current active loans</div>
              <p>Your credit eligibility is being monitored by our AI.</p>
            </div>
          </q-card-section>
        </q-card>
      </div>
    </div>

    <!-- Loan Dialog -->
    <q-dialog v-model="showApplyDialog">
      <q-card dark class="bg-surface" style="min-width: 450px; border-radius: 20px;">
        <q-card-section>
            <div class="text-h6 text-weight-bold">Apply for CBDC Micro-Loan</div>
            <div class="text-caption text-grey-5">Instant approval powered by Sovereign AI</div>
        </q-card-section>

        <q-card-section class="q-pt-none q-gutter-y-md">
            <q-input dark filled v-model.number="loanForm.amount" type="number" label="Requested Amount" prefix="$" />
            <q-select dark filled v-model="loanForm.term" :options="[3, 6, 12, 18, 24]" label="Term (Months)" />

            <div class="bg-grey-10 q-pa-md rounded-borders">
                <div class="row justify-between q-mb-xs">
                    <span class="text-grey-5">Monthly Repayment:</span>
                    <span class="text-weight-bold text-primary">${{ calculatedRepayment }}</span>
                </div>
                <div class="row justify-between">
                    <span class="text-grey-5">AI-Adjusted Interest Rate:</span>
                    <span class="text-weight-bold">{{ calculatedRate }}%</span>
                </div>
            </div>

            <q-btn unelevated color="primary" label="Broadcast Application" @click="handleApply" class="full-width q-py-sm" no-caps rounded :disable="creditScore < 600" />
            <div v-if="creditScore < 600" class="text-caption text-negative text-center">Your AI Index is currently too low for automatic micro-loan approval.</div>
        </q-card-section>
      </q-card>
    </q-dialog>
  </q-page>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useQuasar } from 'quasar'

const $q = useQuasar()
const creditScore = ref(742)
const riskTier = ref('EXCELLENT')
const loading = ref(false)
const showApplyDialog = ref(false)
const loans = ref([])
const loanForm = ref({ amount: 1000, term: 12 })

function getScoreColor(score) {
    if (score > 700) return '#00d2ff'
    if (score > 600) return '#f2c037'
    return '#f44336'
}

const calculatedRate = computed(() => {
    if (creditScore.value > 700) return 4.8
    if (creditScore.value > 600) return 8.5
    return 15.0
})

const calculatedRepayment = computed(() => {
    const p = loanForm.value.amount
    const i = calculatedRate.value / 100 / 12
    const n = loanForm.value.term
    if (!p) return '0.00'
    const payment = (p * i * Math.pow(1 + i, n)) / (Math.pow(1 + i, n) - 1)
    return payment.toFixed(2)
})

function refreshScore() {
    loading.value = true
    setTimeout(() => {
        loading.value = false
        $q.notify({ color: 'primary', message: 'AI Credit Profile Assessment Complete' })
    }, 2000)
}

function handleApply() {
    loans.value.unshift({
        id: Date.now(),
        principal: loanForm.value.amount,
        remaining_balance: loanForm.value.amount,
        rate: calculatedRate.value,
        next_payment: new Date(Date.now() + 30*24*60*60*1000).toLocaleDateString()
    })
    showApplyDialog.value = false
    $q.notify({ color: 'green-4', message: 'Loan Approved & Distributed' })
}
</script>

<style scoped>
.glass-card {
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 20px;
}
.score-display {
    width: 180px;
    height: 180px;
    border: 10px solid #00d2ff;
    border-radius: 50%;
    box-shadow: 0 0 30px rgba(0, 210, 255, 0.2);
}
.bg-surface { background: #111; }
.shadow-glow { box-shadow: 0 4px 30px rgba(0, 210, 255, 0.2); }
.height-100 { height: 100%; }
.rounded-borders { border-radius: 16px; }
</style>
