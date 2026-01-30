<template>
  <q-page class="q-pa-lg bg-dark text-white">
    <!-- Institutional Market Ticker -->
    <div class="row q-col-gutter-sm q-mb-lg no-wrap overflow-hidden" style="border-bottom: 1px solid rgba(255, 255, 255, 0.05); padding-bottom: 15px;">
      <div v-for="ticker in tickers" :key="ticker.name" class="q-px-md row items-center no-wrap">
        <span class="text-caption text-grey-5 q-mr-sm text-weight-bold">{{ ticker.name }}</span>
        <span class="text-subtitle2 text-weight-bolder" :class="ticker.up ? 'text-green-4' : 'text-red-4'">
          {{ ticker.price }}
          <q-icon :name="ticker.up ? 'arrow_drop_up' : 'arrow_drop_down'" size="20px" />
        </span>
      </div>
    </div>

    <div class="row q-col-gutter-lg">
      <!-- Balance Card -->
      <div class="col-12 col-md-4">
        <q-card flat class="bg-primary-gradient text-white shadow-glow q-pa-md" style="border-radius: 20px; min-height: 200px;">
          <q-card-section>
            <div class="row items-center justify-between q-mb-md">
              <div class="text-subtitle1 opacity-80 uppercase tracking-widest text-weight-bold">Current Balance</div>
              <q-icon name="account_balance_wallet" size="28px" />
            </div>
            <div class="text-h2 text-weight-bolder q-mb-xs">${{ parseFloat(balance).toLocaleString() }}</div>
            <div class="text-subtitle2 opacity-70">Sovereign CBDC Reserves</div>
          </q-card-section>

          <q-card-actions align="around" class="q-pt-md">
            <q-btn flat dense icon="add" label="Mint" @click="handleFaucet" no-caps class="bg-white-soft q-px-md" />
            <q-btn flat dense icon="send" label="Send" @click="showSendDialog = true" no-caps class="bg-white-soft q-px-md" />
            <q-btn flat dense icon="call_received" label="Receive" @click="showReceiveDialog = true" no-caps class="bg-white-soft q-px-md" />
          </q-card-actions>
        </q-card>
      </div>

      <!-- Quick Stats / AI Insights -->
      <div class="col-12 col-md-8">
        <div class="row q-col-gutter-lg">
          <div class="col-6">
            <q-card flat class="glass-card q-pa-md height-100 flex flex-center text-center">
              <div>
                <q-icon name="analytics" color="primary" size="32px" class="q-mb-sm" />
                <div class="text-h6 text-weight-bold">Market Health</div>
                <div class="text-green-4 text-weight-bolder">+1.2% (Atomic)</div>
              </div>
            </q-card>
          </div>
          <div class="col-6">
            <q-card flat class="glass-card q-pa-md height-100 flex flex-center text-center cursor-pointer" @click="router.push('/dashboard/loans')">
              <div>
                <q-icon name="psychology" color="accent" size="32px" class="q-mr-sm" />
                <div class="text-h6 text-weight-bold">AI Credit Index</div>
                <div class="text-primary text-h4 text-weight-bolder">{{ aiDetails.score }}</div>
                <div class="text-caption text-grey-5 uppercase tracking-widest">{{ aiDetails.score > 700 ? 'EXCELLENT' : 'GOOD' }}</div>
                <q-tooltip class="bg-dark text-white shadow-24 q-pa-md" style="border: 1px solid #3a7bd5">
                  <div class="text-weight-bold q-mb-xs">Explainable AI Breakdown ($X$)</div>
                  <div class="row q-gutter-x-sm">
                    <span>Telco: {{ (aiDetails.breakdown.telco * 100).toFixed(0) }}%</span>
                    <span>Utility: {{ (aiDetails.breakdown.utility * 100).toFixed(0) }}%</span>
                    <span>Velocity: {{ (aiDetails.breakdown.wallet * 100).toFixed(0) }}%</span>
                  </div>
                </q-tooltip>
              </div>
            </q-card>
          </div>
        </div>
      </div>

      <!-- Transactions List -->
      <div class="col-12">
        <q-card flat class="bg-surface q-pa-md rounded-borders shadow-24">
          <q-card-section class="row items-center justify-between">
            <div class="text-h6 text-weight-bold">Recent Ledger Activity</div>
            <q-btn flat color="primary" label="View Full Explorer" no-caps dense />
          </q-card-section>

          <q-separator dark />

          <q-card-section>
            <q-list dark separator>
              <q-item v-for="tx in transactions" :key="tx.id" class="q-py-md">
                <q-item-section avatar>
                   <q-avatar :color="tx.amount > 0 ? 'green-9' : 'red-9'" text-color="white" :icon="tx.amount > 0 ? 'arrow_downward' : 'arrow_upward'" />
                </q-item-section>
                <q-item-section>
                  <q-item-label class="text-weight-bold">{{ tx.note }}</q-item-label>
                  <q-item-label caption class="text-grey-5">{{ new Date(tx.created_at).toLocaleString() }}</q-item-label>
                </q-item-section>
                <q-item-section side>
                  <div class="text-weight-bolder" :class="tx.amount > 0 ? 'text-green-4' : 'text-red-4'">
                    {{ tx.amount > 0 ? '+' : '' }}{{ tx.amount.toFixed(2) }} CBDC
                  </div>
                </q-item-section>
              </q-item>
            </q-list>

            <div v-if="transactions.length === 0" class="text-center q-pa-xl text-grey-7">
              <q-icon name="receipt_long" size="64px" class="q-mb-md" />
              <div class="text-h6">No atomic transactions in current block</div>
            </div>
          </q-card-section>
        </q-card>
      </div>
    </div>

    <!-- Dialogs remain similar to reference -->
    <q-dialog v-model="showSendDialog">
      <q-card dark class="bg-surface" style="min-width: 400px">
        <q-card-section> <div class="text-h6">Send CBDC</div> </q-card-section>
        <q-card-section class="q-pt-none">
          <q-form @submit="handleSendMoney" class="q-gutter-md">
            <q-input dark filled v-model="sendForm.receiver" label="Receiver Address" hint="CBDC-XXXXX" />
            <q-input dark filled v-model.number="sendForm.amount" type="number" label="Amount" prefix="$" />
            <div class="row justify-end q-gutter-sm">
              <q-btn flat label="Cancel" color="grey" v-close-popup />
              <q-btn unelevated label="Broadcast" type="submit" color="primary" icon-right="send" />
            </div>
          </q-form>
        </q-card-section>
      </q-card>
    </q-dialog>

    <q-dialog v-model="showReceiveDialog">
      <q-card dark class="bg-surface" style="min-width: 400px">
        <q-card-section> <div class="text-h6">Receive CBDC</div> </q-card-section>
        <q-card-section class="text-center">
          <div class="text-subtitle2 text-grey-5 q-mb-sm">Your Atomic Address</div>
          <div class="text-caption font-mono bg-grey-9 q-pa-md rounded-borders text-blue-2" style="border: 1px solid rgba(255, 255, 255, 0.2); word-break: break-all">
            {{ walletAddress }}
          </div>
          <q-btn flat color="primary" label="Copy Address" icon="content_copy" class="q-mt-md" @click="copyAddress" />
        </q-card-section>
        <q-card-actions align="right"> <q-btn flat label="Close" color="grey" v-close-popup /> </q-card-actions>
      </q-card>
    </q-dialog>
  </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useQuasar } from 'quasar'

const router = useRouter()
const $q = useQuasar()
const balance = ref(0)
const walletAddress = ref('CBDC-LOADING')
const transactions = ref([])
const showSendDialog = ref(false)
const showReceiveDialog = ref(false)
const sendForm = ref({ receiver: '', amount: 0 })

const tickers = ref([
  { name: 'CBDC/USD', price: '1.0000', up: true },
  { name: 'IOTA/USD', price: '0.2451', up: true },
  { name: 'BTC/USD', price: '42,150', up: false },
  { name: 'EUR/USD', price: '1.0822', up: true }
])

const aiDetails = ref({
  score: 0,
  breakdown: {
    telco: 0,
    utility: 0,
    wallet: 0,
    social: 0
  }
})

import { getAIScore } from 'src/api'

onMounted(async () => {
  const userData = localStorage.getItem('cbdc_user')
  if (userData) {
    const user = JSON.parse(userData)
    balance.value = user.wallet.balance
    walletAddress.value = user.wallet.walletAddress
    // Mock some transactions for the reference look
    transactions.value = [
      { id: 1, note: 'Sovereign Mint - Testing', amount: 1000.00, created_at: new Date().toISOString() },
      { id: 2, note: 'Atomic Transfer - Gas', amount: -2.50, created_at: new Date(Date.now() - 3600000).toISOString() }
    ]

    // Fetch AI Score
    try {
      const aiData = await getAIScore()
      if (aiData.success) {
        aiDetails.value.score = aiData.score
        aiDetails.value.breakdown = {
          telco: aiData.breakdown.telco_contribution,
          utility: aiData.breakdown.utility_contribution,
          wallet: aiData.breakdown.wallet_velocity,
          social: aiData.breakdown.social_reputation
        }
      }
    } catch (err) {
      console.error('Failed to fetch AI Score:', err)
    }
  }
})

function handleFaucet() {
  balance.value += 1000
  transactions.value.unshift({ id: Date.now(), note: 'Minted from Faucet', amount: 1000, created_at: new Date().toISOString() })
  $q.notify({ color: 'primary', message: '1,000 CBDC Minted Successully' })
}

function handleSendMoney() {
    if (sendForm.value.amount > balance.value) {
        $q.notify({ color: 'negative', message: 'Insufficient balance' })
        return
    }
    balance.value -= sendForm.value.amount
    transactions.value.unshift({ id: Date.now(), note: `Sent to ${sendForm.value.receiver}`, amount: -sendForm.value.amount, created_at: new Date().toISOString() })
    showSendDialog.value = false
    $q.notify({ color: 'green-4', message: 'Transfer broadcasted successfully' })
}

function copyAddress() {
    navigator.clipboard.writeText(walletAddress.value)
    $q.notify({ color: 'primary', message: 'Address copied' })
}
</script>

<style scoped>
.bg-primary-gradient {
  background: linear-gradient(135deg, #00d2ff 0%, #3a7bd5 100%);
}
.glass-card {
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 20px;
}
.bg-surface { background: #111; }
.bg-white-soft { background: rgba(255, 255, 255, 0.15); }
.shadow-glow { box-shadow: 0 4px 20px rgba(0, 210, 255, 0.3); }
.height-100 { height: 100%; }
.rounded-borders { border-radius: 16px; }
.uppercase { text-transform: uppercase; }
.tracking-widest { letter-spacing: 2px; }
</style>
