<template>
  <q-page class="bg-dark-bg q-pa-lg">
    <div class="dashboard-content fade-in-up">
      <!-- Header Section -->
      <div class="row items-center justify-between q-mb-xl">
        <div>
          <h1 class="text-h3 text-weight-bolder text-white q-mb-xs">Welcome back, {{ userName }}!</h1>
          <p class="text-grey-5 text-h6 text-weight-light">Here's what's happening with your wallet today.</p>
        </div>
        <div class="row q-gutter-sm">
          <q-btn icon="add" label="Add Funds" class="btn-primary" rounded no-caps unelevated />
          <q-btn icon="ios_share" label="Export" class="btn-secondary" rounded outline no-caps />
        </div>
      </div>

      <!-- Stats Grid -->
      <div class="row q-col-gutter-lg q-mb-xl">
        <div class="col-12 col-md-4">
          <div class="gradient-card shadow-20 flex flex-col justify-between">
            <div>
              <div class="row justify-between items-center q-mb-md">
                <span class="text-subtitle1 opacity-80">Total Balance</span>
                <q-icon name="account_balance_wallet" size="24px" />
              </div>
              <div class="text-h2 text-weight-bolder q-mb-sm">{{ balance }} {{ currency }}</div>
              <div class="text-subtitle2 opacity-70">≈ $ {{ (parseFloat(balance) * 1.0).toLocaleString() }} USD</div>
            </div>
            <div class="row items-center q-mt-lg">
              <q-icon name="trending_up" color="green-4" size="20px" class="q-mr-xs" />
              <span class="text-green-4 text-weight-bold">+2.4%</span>
              <span class="text-caption q-ml-sm opacity-60">from last month</span>
            </div>
          </div>
        </div>

        <div class="col-12 col-md-4">
          <div class="wallet-card shadow-20">
            <div class="row justify-between items-center q-mb-md">
              <span class="text-subtitle1 text-grey-5">Asset Distribution</span>
              <q-icon name="pie_chart" color="primary" size="24px" />
            </div>
            <div class="q-py-md">
              <div class="row items-center q-mb-sm">
                <div class="col-4 text-grey-5 caption">CBDC</div>
                <div class="col-8">
                  <q-linear-progress :value="0.85" color="primary" rounded class="q-py-xs" />
                </div>
              </div>
              <div class="row items-center q-mb-sm">
                <div class="col-4 text-grey-5 caption">IOTA</div>
                <div class="col-8">
                  <q-linear-progress :value="0.12" color="teal-4" rounded class="q-py-xs" />
                </div>
              </div>
              <div class="row items-center">
                <div class="col-4 text-grey-5 caption">Opal</div>
                <div class="col-8">
                  <q-linear-progress :value="0.03" color="amber-4" rounded class="q-py-xs" />
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="col-12 col-md-4">
          <div class="wallet-card shadow-20 overflow-hidden relative-position">
            <div class="row justify-between items-center q-mb-md">
              <span class="text-subtitle1 text-grey-5">AI Financial Health</span>
              <q-badge color="purple" label="BETA" rounded />
            </div>
            <div class="text-center q-pa-md">
              <div class="text-h2 text-weight-bolder text-white q-mb-xs">7.8</div>
              <div class="text-caption text-grey-5 q-mb-md">Strong Financial Position</div>
              <q-btn flat color="primary" label="View Detailed Analysis" no-caps dense to="/ai-score" />
            </div>
            <div class="absolute-bottom bg-gradient-success" style="height: 4px;"></div>
          </div>
        </div>
      </div>

      <!-- Main Content Grid -->
      <div class="row q-col-gutter-lg">
        <!-- Recent Transactions -->
        <div class="col-12 col-md-8">
          <div class="wallet-card shadow-20 full-height">
            <div class="row justify-between items-center q-mb-lg">
              <h3 class="text-h5 text-white text-weight-bold q-my-none">Recent Transactions</h3>
              <q-btn flat color="grey-5" label="View All" no-caps dense />
            </div>

            <q-list dark separator class="rounded-borders">
              <q-item v-for="tx in transactions" :key="tx.id" class="q-py-md">
                <q-item-section avatar>
                  <q-avatar :color="tx.type === 'In' ? 'green-9' : 'red-9'" text-color="white" :icon="tx.type === 'In' ? 'arrow_downward' : 'arrow_upward'" />
                </q-item-section>
                <q-item-section>
                  <q-item-label class="text-weight-bold">{{ tx.label }}</q-item-label>
                  <q-item-label caption class="text-grey-5">{{ tx.date }} • {{ tx.status }}</q-item-label>
                </q-item-section>
                <q-item-section side>
                  <div class="text-weight-bolder" :class="tx.type === 'In' ? 'text-green-4' : 'text-red-4'">
                    {{ tx.type === 'In' ? '+' : '-' }} {{ tx.amount }} {{ currency }}
                  </div>
                </q-item-section>
              </q-item>
            </q-list>

            <div v-if="transactions.length === 0" class="text-center q-pa-xl">
              <q-icon name="receipt_long" size="64px" color="grey-8" class="q-mb-md" />
              <p class="text-grey-6 text-h6">No transactions found</p>
            </div>
          </div>
        </div>

        <!-- Quick actions / Contacts -->
        <div class="col-12 col-md-4">
          <div class="wallet-card shadow-20 q-mb-lg">
            <h3 class="text-h6 text-white text-weight-bold q-mb-md">Wallet Address</h3>
            <div class="glass-effect q-pa-md row items-center justify-between" style="border-radius: 12px;">
              <code class="text-caption text-grey-4 ellipsis col">{{ walletAddress }}</code>
              <q-btn flat round color="grey-5" icon="content_copy" size="sm" @click="copyAddress" />
            </div>
          </div>

          <div class="wallet-card shadow-20">
            <h3 class="text-h6 text-white text-weight-bold q-mb-md">Quick Send</h3>
            <div class="row q-gutter-x-sm q-mb-md">
              <q-avatar v-for="n in 4" :key="n" size="40px" class="cursor-pointer border-grey">
                <img :src="`https://i.pravatar.cc/100?u=${n}`">
              </q-avatar>
              <q-btn round color="grey-9" icon="add" size="sm" />
            </div>
            <q-input filled dark dense placeholder="Amount to send" class="q-mb-md input-field" />
            <q-btn label="Send Funds" class="full-width btn-primary" no-caps unelevated rounded />
          </div>
        </div>
      </div>
    </div>
  </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useQuasar } from 'quasar'

const $q = useQuasar()

const balance = ref('0.00')
const currency = ref('CBDC')
const walletAddress = ref('CBDC-XXXXXXXXXXXX')
const userName = ref('User')

const transactions = ref([
  { id: 1, label: 'Payroll Received', date: 'Jan 28, 2026', status: 'Completed', amount: '2,500.00', type: 'In' },
  { id: 2, label: 'Cloud Services Payment', date: 'Jan 26, 2026', status: 'Completed', amount: '120.50', type: 'Out' },
  { id: 3, label: 'Internal Transfer', date: 'Jan 25, 2026', status: 'Pending', amount: '500.00', type: 'Out' },
])

onMounted(() => {
  const userData = localStorage.getItem('cbdc_user')
  if (userData) {
    const user = JSON.parse(userData)
    balance.value = user.wallet.balance.toLocaleString(undefined, { minimumFractionDigits: 2 })
    currency.value = user.wallet.currency
    walletAddress.value = user.wallet.walletAddress
    userName.value = user.firstName
  }
})

function copyAddress() {
  navigator.clipboard.writeText(walletAddress.value)
  $q.notify({
    message: 'Address copied to clipboard',
    color: 'primary',
    position: 'bottom'
  })
}
</script>

<style scoped lang="scss">
.dashboard-content {
  max-width: 1400px;
  margin: 0 auto;
}

.bg-dark-bg {
  background: #0a0e27;
}

.border-grey {
  border: 1px solid rgba(255, 255, 255, 0.1);
}

.opacity-80 { opacity: 0.8; }
.opacity-70 { opacity: 0.7; }
.opacity-60 { opacity: 0.6; }

.rounded-borders {
  border-radius: 12px;
}
</style>
