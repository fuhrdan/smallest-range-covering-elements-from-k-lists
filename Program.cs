//*****************************************************************************
//** 632. Smallest Range Covering Elements from K Lists    leetcode          **
//*****************************************************************************


/**
 * Note: The returned array must be malloced, assume caller calls free().
 */
typedef struct {
    int value;
    int row;
    int col;
} HeapNode;

void swap(HeapNode* a, HeapNode* b) {
    HeapNode temp = *a;
    *a = *b;
    *b = temp;
}

// Push a new node into the heap (min-heapify)
void pushHeap(HeapNode* heap, int* heapSize, HeapNode node) {
    int i = (*heapSize)++;
    heap[i] = node;

    // Bubble up
    while (i > 0) {
        int parent = (i - 1) / 2;
        if (heap[parent].value > heap[i].value) {
            swap(&heap[parent], &heap[i]);
            i = parent;
        } else {
            break;
        }
    }
}

// Pop the minimum node from the heap (min-heapify)
HeapNode popHeap(HeapNode* heap, int* heapSize) {
    HeapNode minNode = heap[0];
    heap[0] = heap[--(*heapSize)];

    // Bubble down
    int i = 0;
    while (1) {
        int left = 2 * i + 1;
        int right = 2 * i + 2;
        int smallest = i;

        if (left < *heapSize && heap[left].value < heap[smallest].value) {
            smallest = left;
        }
        if (right < *heapSize && heap[right].value < heap[smallest].value) {
            smallest = right;
        }

        if (smallest != i) {
            swap(&heap[i], &heap[smallest]);
            i = smallest;
        } else {
            break;
        }
    }

    return minNode;
}

int* smallestRange(int** nums, int numsSize, int* numsColSize, int* returnSize) {
    *returnSize = 2;
    int *result = (int*)malloc(2 * sizeof(int));

    // Min-heap to track the smallest element from each list
    HeapNode *heap = (HeapNode*)malloc(numsSize * sizeof(HeapNode));
    int heapSize = 0;

    int maxVal = INT_MIN;

    // Initialize heap with the first element of each list and track the maximum value
    for (int i = 0; i < numsSize; i++) {
        pushHeap(heap, &heapSize, (HeapNode){nums[i][0], i, 0});
        if (nums[i][0] > maxVal) {
            maxVal = nums[i][0];
        }
    }

    // Set initial range to a very large value
    int rangeLeft = 0, rangeRight = INT_MAX;

    // Process the heap
    while (heapSize == numsSize) {
        // Get the smallest element from the heap
        HeapNode minNode = popHeap(heap, &heapSize);

        // Update the range if we found a smaller range
        if (maxVal - minNode.value < rangeRight - rangeLeft) {
            rangeLeft = minNode.value;
            rangeRight = maxVal;
        }

        // Move to the next element in the current row
        if (minNode.col + 1 < numsColSize[minNode.row]) {
            int nextValue = nums[minNode.row][minNode.col + 1];
            pushHeap(heap, &heapSize, (HeapNode){nextValue, minNode.row, minNode.col + 1});
            if (nextValue > maxVal) {
                maxVal = nextValue;
            }
        }
    }

    // Return the smallest range
    result[0] = rangeLeft;
    result[1] = rangeRight;

    // Clean up
    free(heap);
    
    return result;
}