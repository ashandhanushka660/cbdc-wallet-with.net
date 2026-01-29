<template>
  <q-page class="q-pa-lg bg-dark text-white">
    <div class="row q-col-gutter-lg">
      <div class="col-12">
        <div class="row items-center q-mb-xl">
           <q-btn flat round icon="security" color="primary" class="q-mr-md" />
           <h1 class="text-h4 text-weight-bolder">Sovereign Admin Command</h1>
           <q-space />
           <q-chip color="red" text-color="white" icon="priority_high">System Live: High Priority</q-chip>
        </div>

        <!-- System Stats -->
        <div class="row q-col-gutter-md q-mb-xl">
           <div class="col-12 col-sm-3" v-for="stat in systemStats" :key="stat.label">
              <q-card dark flat class="bg-surface q-pa-md border-glow">
                 <div class="text-caption text-grey-5 uppercase">{{ stat.label }}</div>
                 <div class="text-h4 text-weight-bolder">{{ stat.value }}</div>
              </q-card>
           </div>
        </div>

        <q-card flat class="bg-surface rounded-borders">
          <q-card-section class="row items-center">
            <div class="text-h6 text-weight-bold">Customer Directory & Risk Assessment</div>
            <q-space />
            <q-input dark dense filled v-model="search" placeholder="Filter by Wallet ID/Name..." style="width: 300px">
               <template v-slot:append> <q-icon name="search" /> </template>
            </q-input>
          </q-card-section>

          <q-table
            dark
            flat
            :rows="customers"
            :columns="columns"
            row-key="id"
            :filter="search"
            class="bg-darker"
            :pagination="{ rowsPerPage: 10 }"
          >
            <template v-slot:body-cell-score="props">
              <q-td :props="props">
                <q-chip :color="getScoreColor(props.value)" text-color="white" size="sm" class="text-weight-bold">
                  {{ props.value }}
                </q-chip>
              </q-td>
            </template>
            <template v-slot:body-cell-actions="props">
              <q-td :props="props" class="q-gutter-x-sm">
                <q-btn flat dense icon="edit" color="primary" @click="editCustomer(props.row)" />
                <q-btn flat dense icon="monetization_on" color="green" @click="grantLoan(props.row)" />
                <q-btn flat dense icon="block" color="red" />
              </q-td>
            </template>
          </q-table>
        </q-card>
      </div>
    </div>

    <!-- Edit Customer Dialog -->
    <q-dialog v-model="showEditDialog">
       <q-card dark class="bg-surface" style="min-width: 450px">
          <q-card-section>
             <div class="text-h6">Modify Sovereign Parameters</div>
             <div class="text-caption text-grey-5">{{ selectedCustomer?.name }}</div>
          </q-card-section>
          
          <q-card-section class="q-gutter-y-md">
             <q-input dark filled v-model.number="selectedCustomer.score" label="AI Credit Index (Sovereign)" type="number" />
             <q-input dark filled v-model.number="selectedCustomer.balance" label="Account Balance Override" type="number" />
             
             <div class="bg-grey-10 q-pa-sm rounded-borders">
                <q-item tag="label" v-ripple>
                   <q-item-section>
                      <q-item-label>Flag for Regulatory Audit</q-item-label>
                   </q-item-section>
                   <q-item-section side> <q-toggle v-model="selectedCustomer.flagged" color="red" /> </q-item-section>
                </q-item>
             </div>
          </q-card-section>

          <q-card-actions align="right">
             <q-btn flat label="Cancel" color="white" v-close-popup />
             <q-btn unelevated label="Commit Changes" color="primary" @click="saveChanges" />
          </q-card-actions>
       </q-card>
    </q-dialog>

    <!-- Grant Loan Dialog -->
    <q-dialog v-model="showLoanDialog">
       <q-card dark class="bg-surface" style="min-width: 400px">
          <q-card-section>
             <div class="text-h6">Force Grant Emergency Loan</div>
          </q-card-section>
          <q-card-section>
             <q-input dark filled v-model.number="loanAmount" label="Loan Principal" prefix="$" type="number" />
             <p class="q-mt-md text-caption text-grey-5">This action bypasses standard AI protocols and directly injects liquidity into the target wallet.</p>
          </q-card-section>
          <q-card-actions align="right">
             <q-btn flat label="Abort" color="white" v-close-popup />
             <q-btn unelevated label="Inject Liquidity" color="green" @click="confirmLoan" />
          </q-card-actions>
       </q-card>
    </q-dialog>
  </q-page>
</template>

<script setup>
import { ref } from 'vue'
import { useQuasar } from 'quasar'

const $q = useQuasar()
const search = ref('')
const showEditDialog = ref(false)
const showLoanDialog = ref(false)
const selectedCustomer = ref(null)
const loanAmount = ref(5000)

const systemStats = [
   { label: 'Network Liquidity', value: '$84.2M' },
   { label: 'Active Blockchains', value: '4' },
   { label: 'Total Intermediaries', value: '1,242' },
   { label: 'Regulatory Alerts', value: '0' }
]

const columns = [
  { name: 'name', label: 'Full Identity', field: 'name', align: 'left', sortable: true },
  { name: 'id', label: 'Wallet UUID', field: 'id', align: 'left' },
  { name: 'score', label: 'AI Index', field: 'score', align: 'center', sortable: true },
  { name: 'balance', label: 'Ledger Balance', field: 'balance', align: 'right', sortable: true },
  { name: 'actions', label: 'Governance Controls', align: 'center' }
]

const customers = ref([
   { id: 'CBDC-F6B42B12D3FE462F', name: 'Ashan Dhanushka', score: 742, balance: 4500.50, flagged: false },
   { id: 'CBDC-A2B3C4D5E6F7G8H9', name: 'Nissanka Karunaratne', score: 610, balance: 1200.00, flagged: false },
   { id: 'CBDC-Q1W2E3R4T5Y6U7I8', name: 'Dissanayaka Bandara', score: 850, balance: 15400.00, flagged: false },
   { id: 'CBDC-9Z8X7C6V5B4N3M2L', name: 'Samantha Perera', score: 420, balance: 50.00, flagged: true }
])

function getScoreColor(score) {
    if (score > 700) return 'green'
    if (score > 600) return 'blue'
    return 'red'
}

function editCustomer(row) {
    selectedCustomer.value = { ...row }
    showEditDialog.value = true
}

function saveChanges() {
    const index = customers.value.findIndex(c => c.id === selectedCustomer.value.id)
    if (index !== -1) {
        customers.value[index] = { ...selectedCustomer.value }
    }
    showEditDialog.value = false
    $q.notify({ color: 'primary', message: 'Sovereign parameters committed to the ledger.' })
}

function grantLoan(row) {
    selectedCustomer.value = row
    showLoanDialog.value = true
}

function confirmLoan() {
    const index = customers.value.findIndex(c => c.id === selectedCustomer.value.id)
    if (index !== -1) {
        customers.value[index].balance += loanAmount.value
    }
    showLoanDialog.value = false
    $q.notify({ color: 'green', message: `Emergency liquidity of $${loanAmount.value} injected successfully.` })
}
</script>

<style scoped>
.bg-surface { background: #111; }
.bg-darker { background: #0a0a0b; }
.rounded-borders { border-radius: 20px; }
.border-glow { border: 1px solid rgba(0, 210, 255, 0.2); }
.uppercase { text-transform: uppercase; letter-spacing: 1px; }
</style>
