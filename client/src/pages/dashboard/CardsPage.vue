<template>
  <q-page class="q-pa-lg bg-dark text-white">
    <div class="row q-col-gutter-lg">
      <div class="col-12">
        <div class="row items-center justify-between q-mb-xl">
           <div class="text-h4 text-weight-bolder">Sovereign Cards & Accounts</div>
           <q-btn color="primary" icon="add" label="Link New Account" no-caps rounded unelevated />
        </div>

        <div class="row q-col-gutter-xl">
          <!-- Physical/Virtual Cards -->
          <div class="col-12 col-md-4">
             <div class="card-stack relative-position">
                <q-card flat class="payment-card bg-indigo-10 q-pa-lg shadow-glow-indigo">
                   <div class="row justify-between items-start">
                      <q-icon name="wifi" size="24px" class="rotate-90" />
                      <div class="text-weight-bold italic">CBDC PRIME</div>
                   </div>
                   <div style="height: 60px"></div>
                   <div class="text-h5 font-mono tracking-widest q-mb-md">4532 •••• •••• 9912</div>
                   <div class="row justify-between items-end">
                      <div>
                         <div class="text-caption opacity-60">CARD HOLDER</div>
                         <div class="text-subtitle1 text-weight-bold">{{ userName }}</div>
                      </div>
                      <div class="text-right">
                         <div class="text-caption opacity-60">EXPIRES</div>
                         <div class="text-subtitle1 text-weight-bold">12/28</div>
                      </div>
                   </div>
                </q-card>
             </div>
             
             <div class="q-mt-xl bg-surface q-pa-md rounded-borders border-accent">
                <div class="text-subtitle2 text-grey-5 q-mb-md uppercase">Card Controls</div>
                <q-list dark>
                   <q-item tag="label" v-ripple>
                      <q-item-section>
                         <q-item-label>Freeze Card</q-item-label>
                      </q-item-section>
                      <q-item-section side> <q-toggle v-model="cardFrozen" color="red" /> </q-item-section>
                   </q-item>
                   <q-item clickable class="text-primary">
                      <q-item-section avatar> <q-icon name="visibility" /> </q-item-section>
                      <q-item-section> View Full Credentials </q-item-section>
                   </q-item>
                </q-list>
             </div>
          </div>

          <!-- Linked Bank Accounts -->
          <div class="col-12 col-md-8">
             <q-card flat class="bg-surface q-pa-lg rounded-borders height-100">
                <div class="text-h6 text-weight-bold q-mb-md">Linked Financial Institutions</div>
                <q-list dark separator>
                   <q-item v-for="bank in banks" :key="bank.name" class="q-py-lg">
                      <q-item-section avatar>
                         <q-avatar rounded color="grey-10" text-color="white" :icon="bank.icon" />
                      </q-item-section>
                      <q-item-section>
                         <q-item-label class="text-weight-bold">{{ bank.name }}</q-item-label>
                         <q-item-label caption class="text-grey-5">{{ bank.accType }} •••• {{ bank.lastFour }}</q-item-label>
                      </q-item-section>
                      <q-item-section side>
                         <div class="text-white text-weight-bold">${{ bank.balance.toLocaleString() }}</div>
                         <div class="text-caption text-green-4">Connected</div>
                      </q-item-section>
                   </q-item>
                </q-list>

                <div class="q-mt-xl text-center">
                    <p class="text-grey-6">Link your commercial bank accounts for instant CBDC settlement.</p>
                </div>
             </q-card>
          </div>
        </div>
      </div>
    </div>
  </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'

const cardFrozen = ref(false)
const userName = ref('VALUED CUSTOMER')
const banks = ref([
  { name: 'National Savings Bank', icon: 'account_balance', accType: 'Savings', lastFour: '8821', balance: 12450.50 },
  { name: 'Commercial Bank PLC', icon: 'payments', accType: 'Checking', lastFour: '4452', balance: 2100.00 }
])

onMounted(() => {
  const userData = localStorage.getItem('cbdc_user')
  if (userData) {
    const user = JSON.parse(userData)
    userName.value = (user.firstName + ' ' + user.lastName).toUpperCase()
  }
})
</script>

<style scoped>
.payment-card {
  height: 220px;
  border-radius: 20px;
  position: relative;
  overflow: hidden;
  background: linear-gradient(135deg, #1e3a8a 0%, #0c1a4b 100%);
  border: 1px solid rgba(255, 255, 255, 0.1);
}
.shadow-glow-indigo { box-shadow: 0 10px 40px rgba(30, 58, 138, 0.4); }
.bg-surface { background: #111; }
.rounded-borders { border-radius: 20px; }
.border-accent { border: 1px solid rgba(0, 210, 255, 0.2); }
.uppercase { text-transform: uppercase; letter-spacing: 1px; }
.italic { font-style: italic; }
.font-mono { font-family: monospace; }
</style>
