<template>
  <q-page class="q-pa-lg bg-dark text-white">
    <div class="row q-col-gutter-lg">
      <div class="col-12">
        <div class="text-h4 text-weight-bolder q-mb-md">Atomic Ledger History</div>
        <q-card flat class="bg-surface q-pa-md rounded-borders">
          <q-card-section class="row items-center q-pb-none">
            <q-input dark dense filled v-model="filter" placeholder="Search transactions..." class="col-12 col-md-4">
              <template v-slot:append>
                <q-icon name="search" />
              </template>
            </q-input>
            <q-space />
            <q-btn flat icon="filter_list" label="Filter" color="grey-5" no-caps />
            <q-btn flat icon="download" label="Export" color="primary" no-caps />
          </q-card-section>

          <q-card-section>
            <q-table
              dark
              flat
              bordered
              :rows="rows"
              :columns="columns"
              row-key="id"
              class="bg-darker"
              :filter="filter"
            >
              <template v-slot:body-cell-type="props">
                <q-td :props="props">
                  <q-chip
                    :color="props.value === 'Credit' ? 'green-9' : 'red-9'"
                    text-color="white"
                    size="sm"
                    class="text-weight-bold"
                  >
                    {{ props.value }}
                  </q-chip>
                </q-td>
              </template>
              <template v-slot:body-cell-amount="props">
                <q-td :props="props" :class="props.row.type === 'Credit' ? 'text-green-4' : 'text-red-4'" class="text-weight-bold">
                  {{ props.row.type === 'Credit' ? '+' : '-' }}${{ props.value.toFixed(2) }}
                </q-td>
              </template>
              <template v-slot:body-cell-status="props">
                <q-td :props="props">
                   <div class="row items-center">
                     <q-badge rounded color="green" class="q-mr-xs" />
                     {{ props.value }}
                   </div>
                </q-td>
              </template>
            </q-table>
          </q-card-section>
        </q-card>
      </div>
    </div>
  </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'

const filter = ref('')
const rows = ref([])

const columns = [
  { name: 'date', label: 'Timestamp (ISO-8601)', field: 'date', align: 'left', sortable: true },
  { name: 'note', label: 'Description', field: 'note', align: 'left', sortable: true },
  { name: 'type', label: 'Flow', field: 'type', align: 'center', sortable: true },
  { name: 'amount', label: 'Value (CBDC)', field: 'amount', align: 'right', sortable: true },
  { name: 'status', label: 'Verification', field: 'status', align: 'center' }
]

onMounted(() => {
  const userData = localStorage.getItem('cbdc_user')
  if (userData) {
    // Generate some mock history based on the dashboard transactions
    const now = new Date()
    rows.value = [
      { id: 1, date: now.toLocaleString(), note: 'Sovereign Mint - Faucet', type: 'Credit', amount: 1000.00, status: 'Finalized' },
      { id: 2, date: new Date(now - 3600000).toLocaleString(), note: 'Transfer to CBDC-6FB42B12', type: 'Debit', amount: 25.50, status: 'Finalized' },
      { id: 3, date: new Date(now - 86400000).toLocaleString(), note: 'Merchant Payment - Starbucks', type: 'Debit', amount: 5.75, status: 'Finalized' },
      { id: 4, date: new Date(now - 172800000).toLocaleString(), note: 'Direct Deposit - Salary', type: 'Credit', amount: 4500.00, status: 'Finalized' },
      { id: 5, date: new Date(now - 259200000).toLocaleString(), note: 'Gas Fees - Atomic Swap', type: 'Debit', amount: 0.12, status: 'Finalized' }
    ]
  }
})
</script>

<style scoped>
.bg-surface { background: #111; }
.bg-darker { background: #0a0a0b; }
.rounded-borders { border-radius: 20px; }
</style>
